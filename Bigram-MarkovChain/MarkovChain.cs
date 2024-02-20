public class MarkovChain
{
    public MarkovChain(int seed)
    {
        _random = new Random(seed);
    }
    private Dictionary<string,Dictionary<string,int>> _counts = new();
    private readonly Random _random; 
    public void Train(string input){
        var words = input.Split(" ");
        var pairs = words.Pairwise();
        foreach (var wordpair in pairs) {
            AddOrIncrement(wordpair);
        }
    }
    public IEnumerable<string> WordsInChain(string word){
        var exists = _counts.TryGetValue(word,out var dict);
        if(!exists || dict is null) return [];
        return dict.OrderBy(w=>w.Value)
            .Select(w=>w.Key);
    }
    public IEnumerable<string> GenerateText(double temperature){
        var startWord = _counts.Keys.ToArray()[_random.Next(0,_counts.Keys.Count())];
        var nextWord = startWord;
        yield return startWord;
        var advancing=true;
        while (advancing){
            var possibleWords = WordsInChain(nextWord).GetEnumerator();
            advancing = possibleWords.MoveNext();
            while (_random.NextDouble() < temperature && possibleWords.Current != null && advancing ){
                advancing = possibleWords.MoveNext();
            }
            if(!advancing) break;
            nextWord = possibleWords.Current;
            if (nextWord is null){
                break;
            }
            yield return nextWord;
        }
    }

    private void AddOrIncrement((string,string) wordpair){
        var (firstWord,secondWord) = wordpair;
        var innerDict = new Dictionary<string, int>
        {
            { secondWord, 1 }
        };
        var addNewWord = _counts.TryAdd(firstWord,innerDict);
        if (!addNewWord){
            var addSecondword = _counts[firstWord].TryAdd(secondWord,1);
            if(!addSecondword) {
                _counts[firstWord][secondWord] +=1;
            }
        }
    }
}