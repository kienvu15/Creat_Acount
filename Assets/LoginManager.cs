using TMPro;
using System.IO;
using UnityEngine;
using System.Text.RegularExpressions;

public class LoginManager : MonoBehaviour
{
    public GameObject panelLogin, panelRegister, panelUserInfo; 

    public TMP_InputField loginUsername, loginPassword;
    public TMP_Text loginMessage;

    public TMP_InputField registerUsername, registerPassword, registerEmail, registerCharacter, registerPhone;
    public TMP_Text registerMessage;

    public TMP_Text usernameText, emailText, characterText, phoneText;

    private string filePath;

    void Start()
    {
        filePath = Application.dataPath + "/Resources/account.txt";
        ShowLoginPanel();
    }

    public void ShowLoginPanel()
    {
        panelLogin.SetActive(true);
        panelRegister.SetActive(false);

        // Xóa nội dung input và thông báo lỗi
        loginUsername.text = "";
        loginPassword.text = "";
        loginMessage.text = "";
    }


    public void ShowRegisterPanel()
    {
        panelLogin.SetActive(false);
        panelRegister.SetActive(true);

        // Xóa nội dung input và thông báo lỗi
        registerUsername.text = "";
        registerPassword.text = "";
        registerEmail.text = "";
        registerCharacter.text = "";
        registerPhone.text = "";
        registerMessage.text = "";
    }


    public void Register()
    {
        string username = registerUsername.text.Trim();
        string password = registerPassword.text.Trim();
        string email = registerEmail.text.Trim();
        string characterName = registerCharacter.text.Trim();
        string phone = registerPhone.text.Trim();

        if (!Regex.IsMatch(username, "^[a-z0-9]{1,20}$"))
        {
            registerMessage.text = "Username chỉ chứa chữ thường và số (tối đa 20 ký tự)";
            return;
        }

        if (!Regex.IsMatch(password, "^(?=.*[@#$%])[a-zA-Z0-9@#$%]{6,20}$"))
        {
            registerMessage.text = "Password phải có ít nhất 6 ký tự, tối đa 20 và chứa @#$%";
            return;
        }

        if (!Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
        {
            registerMessage.text = "Email không hợp lệ!";
            return;
        }

        if (characterName.Length > 15)
        {
            registerMessage.text = "Character Name không quá 15 ký tự!";
            return;
        }

        if (!Regex.IsMatch(phone, @"^(03|05|07|08|09)[0-9]{8}$"))
        {
            registerMessage.text = "Số điện thoại không hợp lệ!";
            return;
        }

        if (IsUsernameExists(username))
        {
            registerMessage.text = "Username đã tồn tại!";
            return;
        }

        SaveAccount(username, password, email, characterName, phone);
        registerMessage.text = "Đăng ký thành công!";
        ShowLoginPanel();
    }

    private bool IsUsernameExists(string username)
    {
        if (File.Exists(filePath))
        {
            string[] accounts = File.ReadAllLines(filePath);
            foreach (string acc in accounts)
            {
                string[] data = acc.Split('\t');
                if (data[0] == username) return true;
            }
        }
        return false;
    }

    private void SaveAccount(string username, string password, string email, string characterName, string phone)
    {
        string accountData = $"{username}\t{password}\t{email}\t{characterName}\t{phone}";
        File.AppendAllText(filePath, accountData + "\n");
    }

    public void Login()
    {
        string username = loginUsername.text.Trim();
        string password = loginPassword.text.Trim();

        if (File.Exists(filePath))
        {
            string[] accounts = File.ReadAllLines(filePath);
            foreach (string acc in accounts)
            {
                string[] data = acc.Split('\t');
                if (data.Length >= 5 && data[0] == username && data[1] == password)
                {
                    Debug.Log("Đăng nhập thành công!");

                    // Hiển thị thông tin người dùng lên panelUserInfo
                    usernameText.text = "Username: " + data[0];
                    emailText.text = "Email: " + data[2];
                    characterText.text = "Character: " + data[3];
                    phoneText.text = "Phone: " + data[4];

                    // Lưu username để dùng trong game
                    PlayerPrefs.SetString("LoggedInUser", data[0]);
                    PlayerPrefs.Save();

                    // Chuyển sang màn hình thông tin
                    panelLogin.SetActive(false);
                    panelUserInfo.SetActive(true);

                    return;
                }
            }
        }
        loginMessage.text = "Sai username hoặc password!";
    }

    // Xử lý Logout
    public void Logout()
    {
        PlayerPrefs.DeleteKey("LoggedInUser"); // Xóa dữ liệu đăng nhập
        ShowLoginPanel();
    }
}