namespace GRG.LeisureCards.Data
{
    public class MsSqlCustomSqlTypeStrings : CustomSqlTypeStringsBase
    {
        public MsSqlCustomSqlTypeStrings()
        {
            _dictionary.Add(CustomSqlType.NText, "NTEXT");
        }
    }
}
