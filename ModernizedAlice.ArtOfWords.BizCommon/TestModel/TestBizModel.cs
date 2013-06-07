using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModernizedAlice.ArtOfWords.BizCommon.Model;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Marks;
using System.Windows.Media;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Character;

namespace ModernizedAlice.ArtOfWords.BizCommon.TestModel
{
    public class TestBizModel
    {
        private static DocumentModel GetTestDocumentModel()
        {
            DocumentModel doc = new DocumentModel();
            
            doc.Text = 
@"横浜の来季の戦力構想から外れていた鈴木尚外野手（３６）＝本名・鈴木尚典＝が今季限りでの現役引退を決意したことが１４日、明らかになった。球団から要請を受けていた２軍の育成コーチに就任するとみられる。
一時は他球団への移籍も考えていた鈴木尚は「最終的に横浜以外のユニホームを着てプレーする自分が想像できなかった。高校から横浜で育ち、愛着は捨てがたかった。これからは違う形でチームに貢献していきたい」と話した。
鈴木尚は１９９１年に神奈川・横浜高からドラフト４位で大洋（現横浜）に入団。９７、９８年に首位打者を獲得し、９８年の日本シリーズではチームの３８年ぶりの日本一に貢献、最高殊勲選手に選ばれた。通算成績は１５１７試合に出場し、打率３割３厘、１４６本塁打、７００打点。";

            return doc;
        }

        private static MarkManager GetTestMarkManager()
        {
            MarkManager mark = new MarkManager();

            mark.AddMark(new Mark() { HeadCharIndex = 0, TailCharIndex = 1, Brush = new SolidColorBrush(Color.FromArgb(64, 0, 0, 128)) });
            mark.AddMark(new Mark() { HeadCharIndex = 5, TailCharIndex = 6, Brush = new SolidColorBrush(Color.FromArgb(64, 0, 0, 128)) });
            mark.AddMark(new Mark() { HeadCharIndex = 10, TailCharIndex = 10, Brush = new SolidColorBrush(Color.FromArgb(64, 0, 0, 128)) });
            mark.AddMark(new Mark() { HeadCharIndex = 15, TailCharIndex = 15, Brush = new SolidColorBrush(Color.FromArgb(64, 0, 0, 128)) });
            mark.AddMark(new Mark() { HeadCharIndex = 20, TailCharIndex = 20, Brush = new SolidColorBrush(Color.FromArgb(64, 0, 128, 0)) });
            mark.AddMark(new Mark() { HeadCharIndex = 25, TailCharIndex = 30, Brush = new SolidColorBrush(Color.FromArgb(64, 0, 128, 0)) });
            mark.AddMark(new Mark() { HeadCharIndex = 35, TailCharIndex = 40, Brush = new SolidColorBrush(Color.FromArgb(64, 0, 128, 0)) });
            mark.AddMark(new Mark() { HeadCharIndex = 45, TailCharIndex = 50, Brush = new SolidColorBrush(Color.FromArgb(64, 0, 128, 128)) });
            mark.AddMark(new Mark() { HeadCharIndex = 60, TailCharIndex = 70, Brush = new SolidColorBrush(Color.FromArgb(64, 0, 128, 128)) });
            mark.AddMark(new Mark() { HeadCharIndex = 90, TailCharIndex = 100, Brush = new SolidColorBrush(Color.FromArgb(64, 0, 128, 128)) });

            return mark;
        }

        private static CharacterManager GetTestCharacterManager()
        {
            CharacterManager manager = new CharacterManager();

            manager.AddModel(new CharacterModel() { Age = "17", LastName = "有坂", FirstName = "未季", Gender = GenderEnum.Female, Id = 1, IsShokatsuOrder = true, IsValid = true, Symbol = "有", ColorBrush = new SolidColorBrush(Color.FromArgb(200, 255, 0, 0)) });
            manager.AddModel(new CharacterModel() { Age = "15", LastName = "長谷川", FirstName = "勝彦", Gender = GenderEnum.Female, Id = 2, IsRichelieuOrder = true, IsValid = true, Symbol = "長", ColorBrush = new SolidColorBrush(Color.FromArgb(200, 0, 255, 0)) });
            manager.AddModel(new CharacterModel() { Age = "16", LastName = "新谷", FirstName = "亮月", Gender = GenderEnum.Female, Id = 3, IsShokatsuOrder = true, IsValid = true, Symbol = "亮", ColorBrush = new SolidColorBrush(Color.FromArgb(200, 0, 0, 255)) });
            manager.AddModel(new CharacterModel() { Age = "18", LastName = "樫森", FirstName = "灯ノ香", Gender = GenderEnum.Female, Id = 4, IsShokatsuOrder = true, IsValid = true, Symbol = "樫", ColorBrush = new SolidColorBrush(Color.FromArgb(200, 180, 180, 180)) });
            manager.AddModel(new CharacterModel() { Age = "1億", LastName = "土師", FirstName = "現", Gender = GenderEnum.Female, Id = 5, IsBismarckOrder = true, IsValid = true, Symbol = "師", ColorBrush = new SolidColorBrush(Color.FromArgb(200, 100, 160, 180)) });

            return manager;
        }

        public static void PrepareForTest()
        {
            ModelsComposite.DocumentModel = GetTestDocumentModel();
            ModelsComposite.MarkManager = GetTestMarkManager();
            ModelsComposite.CharacterManager = GetTestCharacterManager();
        }

    }
}
