namespace Cial.Helpers
{
    public static class Common
    {
        public static bool IsValidId(int? id)
        {
            if (id == null || id < 1) return false;
            
            return true;
        }
    }
}
