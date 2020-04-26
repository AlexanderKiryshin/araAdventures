using System;
using Assets._Scripts.FakeHexes;

namespace Assets._Scripts.Model
{
    public class HexWithPasses
    {
        public BaseFakeHexType hex;

        private int countPasses;
        public int CountPasses
        {
            get { return countPasses; }
            private set { countPasses = value; }
        }

        public void IncrementCountPasses()
        {
            CountPasses++;
        }
        public HexWithPasses(BaseFakeHexType fakeHex)
        {
            this.hex = fakeHex;
            countPasses = 0;
        }
        public HexWithPasses(int countPasses,BaseFakeHexType fakeHex)
        {
            this.hex = fakeHex;
            this.countPasses = countPasses;
        }
    }
}
