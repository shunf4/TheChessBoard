namespace ChessDotNet
{
    /// <summary>
    /// 枚举类型，标志这个 Move 是什么类型。
    /// </summary>
    [System.Flags]
    public enum MoveType
    {
        Invalid = 1,    //不合法的走子
        Move = 2,       //普通的移动。
        Capture = 4,    //有吃子。
        Castling = 8,   //有易位。
        Promotion = 16  //有晋升。
            //注意，这几个 type 分别占用二进制的不同位，所以可以用“或”来把它们连接起来。

    }
}
