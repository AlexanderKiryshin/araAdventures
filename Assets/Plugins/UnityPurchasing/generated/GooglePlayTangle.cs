#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("JkHYgBqi9LstdUkcWg0lzN4KR3iOKV5kx80ZtCLLImM0gDBEspYdGMXjrs8W8gc4KbIs/ErypO6uvm2D4H0KO8khlgYWgN7/sN8RZ/m2AZs8v7G+jjy/tLw8v7++fAUvSc5okPLBOgw+vtA+piIm2Oz1/LMxGrXt4gOJL+vJRodfy/LwcMrAv9yannYGst97XHJ2HxKDd/qPOhAiq6fAeyEGSe7xuFhAOoeZl5CS7NYBOEkNeSN0RfbLwr+pfN2+df1/DbUvYl2+JBcna+iGjNmIWG05WbUwZ0SUIg7TSkHixR0e2Vpa/LRjG0iqCYlFjjy/nI6zuLeUOPY4SbO/v7+7vr0TUYlz5n1gAGeST6YvXJ15zJ0XTEntELpMkS9sp7y9v76/");
        private static int[] order = new int[] { 4,6,5,7,6,13,13,8,11,11,12,11,13,13,14 };
        private static int key = 190;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
