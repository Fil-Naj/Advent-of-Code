using AdventOfCode.Extensions;

namespace AdventOfCode._2023.Solutions;

internal class Puzzle07 : Puzzle2023<Puzzle07>
{
    private const char Joker = 'J';

    private static readonly Dictionary<char, int> CardWeights = new()
    {
        { 'A', 14 },
        { 'K', 13 },
        { 'Q', 12 },
        { 'J', 11 },
        { 'T', 10 },
        { '9', 9 },
        { '8', 8 },
        { '7', 7 },
        { '6', 6 },
        { '5', 5 },
        { '4', 4 },
        { '3', 3 },
        { '2', 2 }
    };

    internal override void Part1()
    {
        List<Hand> hands = [];
        long totalWinnings = 0L;
        foreach (var game in File)
        {
            var items = game.Split(' ');
            hands.Add(new()
            {
                Cards = items[0],
                WinType = JudgeCard(items[0]),
                Bid = long.Parse(items[1]),
            });
        }

        hands.Sort();
        for (int i = 1; i <= hands.Count; i++)
        {
            totalWinnings += i * hands[i - 1].Bid;
        }

        totalWinnings.Dump("Part 1 Answer");
    }

    private WinType JudgeCard(string hand)
    {
        var distinct = hand.Distinct().Count();

        if (distinct == 5) return WinType.HighCard;
        if (distinct == 4) return WinType.OnePair;
        if (distinct == 1) return WinType.FiveOfAKind; // Fruad

        if (distinct == 2)
        {
            // Four of a Kind or Full House possible
            var firstCardCount = hand.Count(c => c == hand[0]);
            if (firstCardCount == 1 || firstCardCount == 4)
                return WinType.FourOfAKind;
            else 
                return WinType.FullHouse;
        }

        // Determine whether Three of a Kind or Two Pair
        var orderedHand = hand.OrderByDescending(c => c).ToArray();
        var currentCard = orderedHand[0];
        var currentCombo = 1;
        var maxCombo = 0;
        for (int i = 1; i < orderedHand.Length; i++)
        {
            if (orderedHand[i] == currentCard)
            {
                currentCombo++;
            }
            else
            {
                currentCard = orderedHand[i];
                currentCombo = 1;
            }
            maxCombo = Math.Max(maxCombo, currentCombo);
        }

        if (maxCombo == 3) return WinType.ThreeOfAKind;

        return WinType.TwoPair;
    }

    internal override void Part2()
    {
        // J is now a Joker, the least powerful card IN THE WORLD of this puzzle
        CardWeights[Joker] = 1;

        List<Hand> hands = [];
        long totalWinnings = 0L;
        foreach (var game in File)
        {
            var items = game.Split(' ');
            hands.Add(new()
            {
                Cards = items[0],
                WinType = JudgeCardWithJokers(items[0]),
                Bid = long.Parse(items[1]),
            });
        }

        hands.Sort();
        for (int i = 1; i <= hands.Count; i++)
        {
            totalWinnings += i * hands[i - 1].Bid;
        }

        totalWinnings.Dump("Part 2 Answer");
    }

    private WinType JudgeCardWithJokers(string hand)
    {
        var distinct = hand.Distinct().Count();
        var jokerCount = hand.Count(c => c == Joker);

        if (distinct == 1) return WinType.FiveOfAKind; // Fruad

        if (distinct == 5)
        {
            // If five distinct, can only have one or zero jokers
            return jokerCount == 1
                ? WinType.OnePair
                : WinType.HighCard;

        }
        if (distinct == 4)
        {
            // If four distinct, can only have betwen zero to two jokers
            // If one joker, than a pair exists. Turn it into Three of a Kind
            // If two jokers, then the joker is the pair. Turn random card into Three of a Kind
            return jokerCount == 0 
                ? WinType.OnePair
                : WinType.ThreeOfAKind;
        }
        

        if (distinct == 2)
        {
            // Four of a Kind or Full House possible
            // Since there is only two groups, if a joker exists, then we go straight to a Five of a Kind
            // Otherwise, continue as normal
            if (jokerCount > 0) return WinType.FiveOfAKind;

            var firstCardCount = hand.Count(c => c == hand[0]);
            if (firstCardCount == 1 || firstCardCount == 4)
            {
                return WinType.FourOfAKind;
            }
            else
                return WinType.FullHouse;
        }

        // Determine whether Three of a Kind or Two Pair
        var orderedHand = hand.OrderByDescending(c => c).ToArray();
        var currentCard = orderedHand[0];
        var currentCombo = 1;
        var maxCombo = 0;
        for (int i = 1; i < orderedHand.Length; i++)
        {
            if (orderedHand[i] == currentCard)
            {
                currentCombo++;
            }
            else
            {
                currentCard = orderedHand[i];
                currentCombo = 1;
            }
            maxCombo = Math.Max(maxCombo, currentCombo);
        }

        if (maxCombo == 3)
        {
            // Here we have three groups. Possible joker values are 3 or 1
            // If any joker exists, we go straight to Four of a Kind
            return jokerCount > 0
                ? WinType.FourOfAKind
                : WinType.ThreeOfAKind;
        }

        // Possible joker values are 2 or 1
        // If two jokers, we convert one pair into a Four of a Kind
        // If one joker, we convert one pair into a Three of a Kind so then we end up with a Full House
        // Otherwise, stay as is
        if (jokerCount == 2) return WinType.FourOfAKind;
        if (jokerCount == 1) return WinType.FullHouse;

        return WinType.TwoPair;
    }

    public enum WinType
    {
        HighCard, // None, effectively
        OnePair,
        TwoPair,
        ThreeOfAKind,
        FullHouse,
        FourOfAKind,
        FiveOfAKind // Bruh imagine you would be thrown on INSTANTANEOUSLY on one deck tables
    }

    public class Hand : IComparable<Hand>
    {
        public string? Cards { get; set; }
        public WinType WinType { get; set; }
        public long Bid { get; set; }

        int IComparable<Hand>.CompareTo(Hand? other)
        {
            if (WinType > other!.WinType) return 1;
            else if (WinType < other.WinType) return -1;

            for (int c = 0; c < Cards!.Length; c++)
            {
                if (CardWeights[Cards[c]] > CardWeights[other.Cards![c]]) return 1;
                else if (CardWeights[Cards[c]] < CardWeights[other.Cards![c]]) return -1;
            }

            return 0;
        }
    }
}
