using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Windows;

namespace ModernizedAlice.ArtOfWords.BizCommon.Util
{
    /// <summary>
    /// ファイルに適正な権限があるかどうか調べるチェッカー
    /// </summary>
    public static class FileAuthorizeChecker
    {
        /// <summary>
        /// アクセス権限を確認してなければ直す
        /// </summary>
        public static void CheckAndRepairAuthorize()
        {
            string dirPath = CommonDirectoryUtil.GetCommonProgramDataVendorPath();

            if (Directory.Exists(dirPath) == false)
            {
                return;
            }

            if (HasWriteAuth(dirPath))
            {
                return;
            }

            var repairAnswer = MessageBox.Show(Application.Current.MainWindow,
                                                "アプリケーションのフォルダーに書き込みができません。\nフォルダーの設定を修正してもよろしいですか？\n" +
                                               "(修正しないとアプリケーションが動作しない可能性があります）",
                                                "確認",
                                                MessageBoxButton.YesNo, 
                                                MessageBoxImage.Question);

            if (repairAnswer == MessageBoxResult.Yes)
            {
                MessageBox.Show(Application.Current.MainWindow,
                    "この後、ユーザー権限昇格のためのメッセージがでますので「はい」を押して進めてください。",
                    "情報",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                SetAuthorize();

                MessageBox.Show(Application.Current.MainWindow,
                    "完了しました！",
                    "情報",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }

            return;
        }

        /// <summary>
        /// 権限を設定する
        /// </summary>
        private static void SetAuthorize()
        {
            Assembly myAssembly = Assembly.GetEntryAssembly();
            
            string path = myAssembly.Location;
            var filePath = Path.Combine(Path.GetDirectoryName(path), @"SupportBin\DirectoryAuthorizationsRepairer.exe");

            //プログラムがあるか調べる
            if (!System.IO.File.Exists(filePath))
            {
                // なきゃしゃーない（◞‸◟）　妨害せず一縷の望みにかける。
                return;
            }

            // 我々の作ったファイルでなければ権限与えたらダメ。
            if (IsValidFile(filePath) == false)
            {
                return;
            }

            System.Diagnostics.ProcessStartInfo psi =
                new System.Diagnostics.ProcessStartInfo();
            //ShellExecuteを使う。デフォルトtrueなので、必要はない。
            psi.UseShellExecute = true;
            //昇格して実行するプログラムのパスを設定する
            psi.FileName = filePath;
            //動詞に「runas」をつける
            psi.Verb = "runas";

            try
            {
                //起動する
                System.Diagnostics.Process p = System.Diagnostics.Process.Start(psi);
                //終了するまで待機する
                p.WaitForExit();
            }
            catch (System.ComponentModel.Win32Exception)
            {
            }
        }

        /// <summary>
        /// ユーザーにディレクトリの書き込み権限があるかどうか確認する
        /// </summary>
        /// <returns>権限があればtrue, なければfalse</returns>
        public static bool HasWriteAuth(string dirPath)
        {
            SecurityIdentifier users = new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null);

            //既存のアクセス権を調べる
            DirectorySecurity securities = Directory.GetAccessControl(dirPath);
            var rules = securities.GetAccessRules(true, true, typeof(SecurityIdentifier));

            foreach (FileSystemAccessRule rule in rules)
            {
                if (rule.IdentityReference != users)
                {
                    continue;
                }

                if (rule.FileSystemRights.HasFlag(FileSystemRights.Write))
                {
                    return true;
                }
                
            }

            return false;
        }

        /// <summary>
        /// ファイルのチェックサムを見て、正当なファイルかどうか確認
        /// </summary>
        /// <param name="file">ファイルパス</param>
        /// <returns>チェックサム</returns>
        private static bool IsValidFile(string filepath)
        {
            string validCheckSum = "f6a0e03aa7040c507f4f86e98dbeb26d";

            using (var fs = new FileStream(filepath, FileMode.Open))
            {
                string md5sum = BitConverter.ToString(MD5.Create().ComputeHash(fs)).ToLower().Replace("-", "");

                fs.Close();

                return md5sum == validCheckSum;
            }
        }
    }
}
