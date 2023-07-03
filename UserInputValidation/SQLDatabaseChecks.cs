namespace UserInputValidation
{
    public class SQLDatabaseChecks
    {
        public static bool IsLowerEqualThanSQLDatabaseMinimum(DateTime DateToCheck)
        {
            if (DateToCheck <= System.Data.SqlTypes.SqlDateTime.MinValue.Value)
                return true;

            return false;
        }
    }
}
