using SpringCard.LibCs;
using SpringCard.AppleVas;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SpringCard.LibCs.Windows;
using System.IO;

namespace PassKitRdr
{
    public partial class AppleVASConfigForm : Form
    {
        /* Test data */
        const string TestMerchantName = "pass.com.springcard.springblue.generic";
        const string TestKeyPrivate = "MHcCAQEEICp+PT7K8FQSOi2HED1Ar5RqxxN2EkiKJMCSfaL4htYNoAoGCCqGSM49AwEHoUQDQgAE9RCZaHxXUIjQFQnwKmq6+cVqFBNO6ZKQmekosMQRZmutPs8szUsiLokILdaiT/7F5qUl8qSfEvlocYy6z98jIw==";
        // const string TestKeyPublic = "MDkwEwYHKoZIzj0CAQYIKoZIzj0DAQcDIgAD9RCZaHxXUIjQFQnwKmq6+cVqFBNO6ZKQmekosMQRZms=";

        byte[] merchant1Id = null;
        byte[] merchant1PrivateKey = null;
        AppleVasConfig config1;
        byte[] merchant2Id = null;
        byte[] merchant2PrivateKey = null;
        AppleVasConfig config2;

        public AppleVASConfigForm()
        {
            InitializeComponent();
        }

        private void AppleVASConfigForm_Load(object sender, EventArgs e)
        {
            eMerchant1Name.Text = AppConfig.ReadSettingString("MerchantName");
            ePrivateKey1.Text = AppConfig.ReadSettingString("PrivateKey");
            if ((eMerchant1Name.Text == "") && (ePrivateKey1.Text == ""))
                lkSetDefault_LinkClicked(sender, null);

            eMerchant2Name.Text = AppConfig.ReadSettingString("MerchantName2");
            ePrivateKey2.Text = AppConfig.ReadSettingString("PrivateKey2");
        }

        public AppleVasTerminalConfig GetConfig()
        {
            eMerchant1Name.Text = AppConfig.ReadSettingString("MerchantName");
            ePrivateKey1.Text = AppConfig.ReadSettingString("PrivateKey");

            eMerchant2Name.Text = AppConfig.ReadSettingString("MerchantName2");
            ePrivateKey2.Text = AppConfig.ReadSettingString("PrivateKey2");

            string json = $@"{{
                ""P2"" : ""FullVAS"",
                ""Capabilities"": ""SingleMode"",
	            ""Merchants"" : [";

            if ((eMerchant1Name.Text == "") && (ePrivateKey1.Text == ""))
            {
                lkSetDefault_LinkClicked(null, null);
            }

            json += $@"
		            {{
			            ""Name"" : ""{eMerchant1Name.Text}"",
			            ""PrivateKey"" : ""{ePrivateKey1.Text}"",
			            ""Url"": ""https://springpass.springcard.com""
                    }}";
	          

            if ((eMerchant2Name.Text != "") && (ePrivateKey2.Text != ""))
            {
                json += $@",
		            {{
			            ""Name"" : ""{eMerchant2Name.Text}"",
			            ""PrivateKey"" : ""{ePrivateKey2.Text}"",
			            ""Url"": """"
                    }}";
            }

            json += $@"
              ],
	            ""Description"" : ""FullVAS, DualMode, 2 merchant IDs with 2nd matching, merchants have an URL""
               }}";

            AppleVasTerminalConfig config = AppleVasTerminalConfig.LoadFromJson(json);
            return config;
        }

        private void lkSetDefault_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Logger.Debug("Set default merchant name=" + TestMerchantName);
            eMerchant1Name.Text = TestMerchantName;
            string str = cut(TestKeyPrivate);
            Logger.Debug("Set default private key=\n" + str);
            ePrivateKey1.Text = str;

            eMerchant2Name.Text = "";
            ePrivateKey2.Text = "";
        }


        private void eMerchantName1_TextChanged(object sender, EventArgs e)
        {
            if (eMerchant1Name.Text == "")
            {
                merchant1Id = null;
                eMerchant1Id.Text = "";
                AppConfig.WriteSettingString("MerchantName", "");
                return;
            }

            merchant1Id = AppleVasConfig.ComputeId(eMerchant1Name.Text);
            Logger.Trace("MerchantId#1={0}", BinConvert.ToHex(merchant1Id));
            eMerchant1Id.Text = BinConvert.ToHex_nice(merchant1Id, ":", "", 0);
            AppConfig.WriteSettingString("MerchantName", eMerchant1Name.Text);
        }

        private void eMerchant2Name_TextChanged(object sender, EventArgs e)
        {
            if (eMerchant2Name.Text == "")
            {
                merchant2Id = null;
                eMerchant2Id.Text = "";
                AppConfig.WriteSettingString("MerchantName2", "");
                return;
            }

            merchant2Id = AppleVasConfig.ComputeId(eMerchant2Name.Text);
            Logger.Trace("MerchantId#2={0}", BinConvert.ToHex(merchant2Id));
            eMerchant2Id.Text = BinConvert.ToHex_nice(merchant2Id, ":", "", 0);
            AppConfig.WriteSettingString("MerchantName2", eMerchant2Name.Text);
        }


        private void ePrivateKey1_TextChanged(object sender, EventArgs e)
        {
            merchant1PrivateKey = null;
            ePublicKey1.Text = "";

            if (ePrivateKey1.Text == "")
            {
                AppConfig.WriteSettingString("PrivateKey", "");
                return;
            }

            string str = ePrivateKey1.Text;
            str = str.Replace(Environment.NewLine, "");
            str = str.Trim();

            if (str.Length == 0)
                return;

            try
            {
                merchant1PrivateKey = StrUtils.Base64Decode(str);
            }
            catch
            {
                return;
            }

            if (AppleVasConfig.ValidatePrivateKey(merchant1PrivateKey, out uint keyId, out byte[] publicKey))
            {
                Logger.Trace("PrivateKey#1={0}", BinConvert.ToHex(merchant1PrivateKey));
                Logger.Trace("KeyId#1={0}", BinConvert.ToHex(keyId));
                Logger.Trace("PublicKey#1(raw)={0}", BinConvert.ToHex(publicKey));
                byte[] publicKeyPem = AppleVasConfig.EncodePublicKeyPem(publicKey);
                Logger.Trace("PublicKey#1(pem)={0}", BinConvert.ToHex(publicKeyPem));
                ePublicKey1.Text = StrUtils.Base64Encode(publicKeyPem);
                AppConfig.WriteSettingString("PrivateKey", ePrivateKey1.Text);
            }
            else
            {
                merchant2PrivateKey = null;
            }
        }

        private void ePrivateKey2_TextChanged(object sender, EventArgs e)
        {
            merchant2PrivateKey = null;
            ePublicKey2.Text = "";

            if (ePrivateKey2.Text == "")
            {
                AppConfig.WriteSettingString("PrivateKey2", "");
                return;
            }

            string str = ePrivateKey2.Text;
            str = str.Replace(Environment.NewLine, "");
            str = str.Trim();

            if (str.Length == 0)
                return;

            try
            {
                merchant2PrivateKey = StrUtils.Base64Decode(str);
            }
            catch
            {
                return;
            }

            if (AppleVasConfig.ValidatePrivateKey(merchant2PrivateKey, out uint keyId, out byte[] publicKey))
            {
                Logger.Trace("PrivateKey#2={0}", BinConvert.ToHex(merchant2PrivateKey));
                Logger.Trace("KeyId#2={0}", BinConvert.ToHex(keyId));
                Logger.Trace("PublicKey#2(raw)={0}", BinConvert.ToHex(publicKey));
                byte[] publicKeyPem = AppleVasConfig.EncodePublicKeyPem(publicKey);
                Logger.Trace("PublicKey#2(pem)={0}", BinConvert.ToHex(publicKeyPem));
                ePublicKey2.Text = StrUtils.Base64Encode(publicKeyPem);
                AppConfig.WriteSettingString("PrivateKey2", ePrivateKey2.Text);
            }
            else
            {
                merchant2PrivateKey = null;
            }
        }
        string cut(string str, int lineLength = 64)
        {
            string result = "";

            while (str.Length > lineLength)
            {
                result += str.Substring(0, lineLength);
                str = str.Substring(lineLength);
                if (str.Length > 0)
                    result += Environment.NewLine;
            }
            if (str.Length > 0)
                result += str;

            return result;
        }

        private void lkClose_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Close();
        }
    }
}
