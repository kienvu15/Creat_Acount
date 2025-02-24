using TMPro; // Thêm thư viện TextMeshPro
using System;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class RegisterManager : MonoBehaviour
{
    public TMP_InputField usernameInput, passwordInput, emailInput, characterNameInput, phoneInput;
    public TMP_Text errorMessage;
    private string filePath;

    void Start()
    {
        filePath = Application.persistentDataPath + "/account.txt"; // Lưu file trong thư mục an toàn
    }

    public void RegisterAccount()
    {
        string username = usernameInput.text.Trim();
        string password = passwordInput.text.Trim();
        string email = emailInput.text.Trim();
        string characterName = characterNameInput.text.Trim();
        string phone = phoneInput.text.Trim();

        // Kiểm tra điều kiện hợp lệ
        if (!Regex.IsMatch(username, "^[a-z0-9]{1,20}$"))
        {
            errorMessage.text = "Username chỉ gồm chữ thường và số, tối đa 20 ký tự!";
            return;
        }
        if (!Regex.IsMatch(password, @"^(?=.*[a-zA-Z])(?=.*\d)(?=.*[@#$%]).{6,20}$"))
        {
            errorMessage.text = "Password phải có chữ, số, ký tự đặc biệt (@#$%), 6-20 ký tự!";
            return;
        }
        if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            errorMessage.text = "Email không hợp lệ!";
            return;
        }
        if (characterName.Length > 15)
        {
            errorMessage.text = "Character Name tối đa 15 ký tự!";
            return;
        }
        if (!Regex.IsMatch(phone, @"^(03|05|07|08|09)\d{8}$"))
        {
            errorMessage.text = "Số điện thoại không hợp lệ!";
            return;
        }

        // Nếu hợp lệ, ghi vào file
        string accountData = $"{username}\t{password}\t{email}\t{characterName}\t{phone}";

        try
        {
            File.AppendAllText(filePath, accountData + Environment.NewLine);
            Debug.Log("Đăng ký thành công!");
            errorMessage.text = "Đăng ký thành công!";
            // Chuyển sang màn hình đăng nhập (Sẽ làm ở bước tiếp theo)
        }
        catch (Exception e)
        {
            errorMessage.text = "Lỗi khi lưu tài khoản: " + e.Message;
        }
    }
}
