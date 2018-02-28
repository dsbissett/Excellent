namespace Excellent.Wpf.ViewModels.Implementations
{
    using System.Collections;
    using System.Collections.Generic;
    using Interfaces;
    using Resources;

    public class MainViewModel : IMainViewModel
    {
        public IEnumerable<DictionaryEntry> Images;

        public IEnumerable<DictionaryEntry> Audio;

        public MainViewModel()
        {
            this.Images = Combinator.Images.Value;

            this.Audio = Combinator.Audio.Value;
        }
    }
}
