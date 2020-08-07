namespace Pudicitia.Common.Queries
{
    public class PaginationOption
    {
        private int index = 1;
        private int size = 10;

        public int Index
        {
            get => index;
            set => index = value > 1 ? value : index;
        }

        public int Size
        {
            get => size;
            set => size = value > 0 && value <= 100 ? value : size;
        }

        public int Offset => (Index - 1) * Size;

        public int Limit => Size;
    }
}