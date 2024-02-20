using FluentAssertions;

namespace BigramMarkovChainTests;

public class UnitTest1
{
    [Fact]
    public void Untrained_Model_Returns_No_Chain()
    {
        var markovChain = new MarkovChain(42);
        var wordsInChain = markovChain.WordsInChain("test");
        wordsInChain.Should().BeEmpty();
    }

    [Fact]
    public void Hello_World_Is_Tokenized(){
        var markovChain = new MarkovChain(42);
        markovChain.Train("Hello world");
        var wordsInChain = markovChain.WordsInChain("Hello");
        wordsInChain.Should().BeInAscendingOrder("World");
    }

    [Fact]
    public void The_Model_Repeats_Its_Input(){
        var markovChain = new MarkovChain(42);
        markovChain.Train("Hello world");
        var generated = markovChain.GenerateText(0);
        string.Join(' ',generated).Should().Be("Hello world");
    }

    [Fact]
    public void Longer_Inputs_Give_Longer_output(){
        var markovChain = new MarkovChain(42);
        var inputStory = """Once upon a time there was a young boy named Jack he lived in a small village nestled at the foot of a towering mountain every day Jack would gaze up at the mountain dreaming of what lay beyond one day he gathered his courage and set out on a journey up the mountain as he climbed higher and higher he encountered many challenges wild animals steep cliffs and raging rivers but Jack pressed on determined to reach the top finally after days of climbing he reached the summit and what he saw took his breath away a vast expanse of green valleys and sparkling lakes stretched out before him it was the most beautiful sight he had ever seen Jack realized that the mountain was just the beginning of his adventure and there was a whole world waiting for him to explore with a smile on his face he began his descent ready to embrace whatever lay ahead""";
        markovChain.Train(inputStory);
        var generated = markovChain.GenerateText(0.0);
        var text = generated.Take(15);
        Console.WriteLine(string.Join(' ',text));
        text.Count().Should().BeGreaterThan(10);

    }

}