namespace ChessDotNet
{
    /// <summary>
    /// 是一个枚举类型，记录了是否易位以及易位的三种类型：不易位，短易位，长易位。用来作 MoreDetailedMove 的属性。
    /// </summary>
    public enum CastlingType
    {
        None,
        KingSide,
        QueenSide
    }
}
