namespace Excellent.Wpf.Resources
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Globalization;
    using System.Linq;
    using Properties;

    public class Combinator
    {
        public static Lazy<IEnumerable<DictionaryEntry>> Audio;

        public static Lazy<IEnumerable<DictionaryEntry>> Images;

        static Combinator()
        {
            var resourceSet = Resources.ResourceManager.GetResourceSet(CultureInfo.CurrentCulture, true, true);

            Images = new Lazy<IEnumerable<DictionaryEntry>>(() => resourceSet.Cast<DictionaryEntry>().Where(x => x.Value is Image));

            Audio = new Lazy<IEnumerable<DictionaryEntry>>(() => resourceSet.Cast<DictionaryEntry>().Where(x => x.Value is byte[]));
        }

        public void Play()
        {
        }
    }
}