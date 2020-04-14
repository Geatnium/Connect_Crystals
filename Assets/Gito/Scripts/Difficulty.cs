
// 難易度の種類
public enum Difficult
{
    Easy, Hard
}

// 難易度を管理
public class Difficulty
{
    // 難易度
    private static Difficult _difficult = Difficult.Easy;
    public static Difficult difficult
    {
        get { return _difficult; }
        set { _difficult = value; }
    }
}
