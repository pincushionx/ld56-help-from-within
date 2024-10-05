namespace Oneill {
    public static class IntUtil {
        public static int Clamp(int n, int min, int max) {
            if (n < min)
                return min;
            if (n > max)
                return max;
            return n;
        }
        //https://www.geeksforgeeks.org/count-set-bits-in-an-integer/
        public static int CountSetBits(int n)
        {
            int count = 0;
            while (n > 0)
            {
                count += n & 1;
                n >>= 1;
            }
            return count;
        }
	}
}
