using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace UniversityApp
{
    public partial class Register : Form
    {
        Form form;
        private string filepath = "users.txt";
        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Convert the input string to a byte array and compute the hash.
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                // Convert the byte array to a string.
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                    builder.Append(bytes[i].ToString("x2"));
                return builder.ToString();
            }
        }
        public Register(Login Login)
        {
            this.form = Login;
            InitializeComponent();
        }
        private void EnsureFileExists()
        {
            if (!File.Exists(filepath))
            {
                File.Create(filepath).Close(); // Create and immediately close the file
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                EnsureFileExists(); // Ensure the file exists before accessing it
                string username = textBox1.Text;
                string password = textBox2.Text;
                string conPass = textBox3.Text;
                string fullName = textBox4.Text;
                string email = textBox6.Text;
                string idNum = textBox5.Text;
                if (string.IsNullOrWhiteSpace(username))
                    throw new ArgumentException("Username cannot be empty!");
                if (string.IsNullOrWhiteSpace(password))
                    throw new ArgumentException("Password cannot be empty!");
                if (string.IsNullOrWhiteSpace(conPass))
                    throw new ArgumentException("Confirm Password cannot be empty!");
                if (!password.Equals(conPass))
                    throw new InvalidOperationException("Your passwords don't match!");
                if (string.IsNullOrWhiteSpace(fullName))
                    throw new ArgumentException("Full Name cannot be empty!");
                if (string.IsNullOrWhiteSpace(email))
                    throw new ArgumentException("Email cannot be empty!");
                if (string.IsNullOrWhiteSpace(idNum))
                    throw new ArgumentException("ID Number cannot be empty!");
                if (!email.Contains("@") || !email.Contains("."))
                    throw new ArgumentException("Please enter a valid email address!");
                if (idNum.Length != 9)
                    throw new ArgumentException("ID Number must be 9 digits long!");
                string groupType = null;
                if (radioButton1.Checked) groupType = "Student";
                else if (radioButton2.Checked) groupType = "Srudent Teacher";
                else if (radioButton4.Checked) groupType = "Professor";
                else if (radioButton3.Checked) groupType = "Head of Department";
                if (groupType == null)
                    throw new ArgumentException("Please select a group type!");
                var lines = File.ReadAllLines(filepath);
                foreach (var line in lines)
                {
                    if (line.Contains($"Username: {username},") ||
                        line.Contains($"Password: {password},") ||  // optional security concern: you might want to hash this instead!
                        line.Contains($"ID: {idNum}"))
                    {
                        throw new InvalidOperationException("A user with the same username, password, or ID already exists!");
                    }
                }
                string userEntry = $"Username: {username}, Password: {password}, Full Name: {fullName}, Email: {email}, ID: {idNum}, Group: {groupType}, Approved: Pending";
                File.AppendAllLines(filepath, new[] { userEntry });
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
                textBox5.Clear();
                textBox6.Clear();
                radioButton1.Checked = radioButton2.Checked = radioButton3.Checked = radioButton4.Checked = false;
                MessageBox.Show("Registration successful! Please wait for a Head of Department to approve your registration.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                form.Show(); // Show the Login form after successful registration
                this.Close(); // Close the Register form
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Operation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            form.Show(); // Use the instance of Login passed to the Register constructor  
            this.Close();
        }
    }
}


