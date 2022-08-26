using System;
using System.Windows.Forms;
using SteamKit2;

namespace IdleMasterExtended
{
    public partial class frmLogin : Form
    {
        private SteamClient steamClient;
        private CallbackManager callbackManager;
        private SteamUser steamUser;

        private string steamUsername;
        private string steamPassword;
        private string steamGuardCode;

        private SteamID steamUserID;
        private string steamUserNonce;
        private string steamVanityURL;
        private EUniverse steamClientUniverse;

        private uint steamLoginKeyUniqueID;
        

        private bool isRunning;

        public frmLogin()
        {
            InitializeComponent();

            // Initialize the client and user objects to handle the login
            steamClient = new SteamClient();
            callbackManager = new CallbackManager(steamClient);
            steamUser = steamClient.GetHandler<SteamUser>();

            // Set up the callback manager for events we need
            callbackManager.Subscribe<SteamClient.ConnectedCallback>(OnConnected);
            callbackManager.Subscribe<SteamClient.DisconnectedCallback>(OnDisconnected);

            callbackManager.Subscribe<SteamUser.LoggedOnCallback>(OnLoggedOn);
            callbackManager.Subscribe<SteamUser.LoggedOffCallback>(OnLoggedOff);
            callbackManager.Subscribe<SteamUser.UpdateMachineAuthCallback>(OnMachineAuth);

            callbackManager.Subscribe<SteamUser.LoginKeyCallback>(OnLoginKeyReceived);
            callbackManager.Subscribe<SteamUser.SessionTokenCallback>(OnSessionTokenCallback);
            callbackManager.Subscribe<SteamUser.WebAPIUserNonceCallback>(OnWebAPIUserNonceCallback);
        }

        private void OnWebAPIUserNonceCallback(SteamUser.WebAPIUserNonceCallback callback)
        {
            MessageBox.Show("I got my Web API Nonce callback! " + callback);
        }

        void OnConnected(SteamClient.ConnectedCallback callback)
        {
            // TODO: Sentry file?

            steamUser.LogOn(new SteamUser.LogOnDetails
            {
                Username = steamUsername,
                Password = steamPassword,
                AuthCode = steamGuardCode,
                TwoFactorCode = steamGuardCode,
            });
        }

        void OnDisconnected(SteamClient.DisconnectedCallback callback)
        {
            MessageBox.Show("Disconnected from Steam..." + callback.ToString());
            isRunning = false;
        }


        void OnLoggedOn(SteamUser.LoggedOnCallback callback)
        {

            if (callback.Result != EResult.OK)
            {
                MessageBox.Show("Unable to logon to Steam: " + callback.Result.ToString());

                isRunning = false;
                return;
            }

            MessageBox.Show("Logged on to Steam as user: " + steamUser.SteamID);

            steamUserID = steamUser.SteamID;
            steamClientUniverse = steamClient.Universe;
            steamUserNonce = callback.WebAPIUserNonce;
            steamVanityURL = callback.VanityURL;

        }

        void OnLoggedOff(SteamUser.LoggedOffCallback callback)
        {
            MessageBox.Show("Disconnected from Steam..." + callback.ToString());
            isRunning = false;
        }

        async void OnLoginKeyReceived(SteamUser.LoginKeyCallback callback)
        {
            steamLoginKeyUniqueID = callback.UniqueID;
            var webApiUserNonceCallback = await steamUser.RequestWebAPIUserNonce();
            steamUserNonce = webApiUserNonceCallback.Nonce;
        }

        void OnSessionTokenCallback(SteamUser.SessionTokenCallback callback)
        {
            MessageBox.Show("I got my session token! " + callback.SessionToken);
        }

        void OnMachineAuth(SteamUser.UpdateMachineAuthCallback callback)
        {
            MessageBox.Show("Sentry file returned..." + callback.ToString());
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            buttonLogin.Enabled = false;

            steamUsername = textBoxUsername.Text;
            steamPassword = textBoxPassword.Text;
            
            if (checkBoxSteamGuard.Checked)
            {
                steamGuardCode = textBoxSteamGuard.Text;
            }
            
            isRunning = true;
            steamClient.Connect();

            while (isRunning)
            {
                // in order for the callbacks to get routed, they need to be handled by the manager
                callbackManager.RunWaitCallbacks(TimeSpan.FromSeconds(1));
            }

            buttonLogin.Enabled = true;
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (textBoxPassword.PasswordChar == '*')
            {
                textBoxPassword.PasswordChar = '\0';
            }
            else
            {
                textBoxPassword.PasswordChar = '*';
            }
        }
    }
}
