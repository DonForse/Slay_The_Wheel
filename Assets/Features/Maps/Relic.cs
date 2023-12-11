using System;

namespace Features.Maps
{
    [Serializable]
    public class Relic
    {
        public RelicScriptableObject RelicBase;

        public Relic(RelicScriptableObject r )
        {
            RelicBase = r;
        }
    }
}