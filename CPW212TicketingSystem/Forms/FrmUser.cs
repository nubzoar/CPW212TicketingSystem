﻿using System;
using System.Windows.Forms;

namespace CPW212TicketingSystem
{
    public partial class FrmUser : Form
    {
        public FrmUser()
        {
            InitializeComponent();
        }

        private void BtnRegisterOrUpdate_Click(object sender, EventArgs e)
        {
            if (!FieldsAreValid())
            {
                return;
            }

            if (LstUser.SelectedIndex < 0)
            {
                User newUser = new User(TxtUsername.Text, TxtPassword.Text, TxtFirstName.Text, TxtLastName.Text, (Role)CboRole.SelectedItem);

                try
                {
                    UserDB.Add(newUser);
                    MessageBox.Show("Registration successful");
                    RefreshForm();
                }
                catch
                {
                    MessageBox.Show("Error: Registration not successful. This username already exists.");
                }
            }
            else
            {
                User selUser = (User)LstUser.SelectedItem;
                selUser.Role = (Role)CboRole.SelectedItem;
                selUser.FirstName = TxtFirstName.Text;
                selUser.LastName = TxtLastName.Text;
                selUser.Username = TxtUsername.Text;
                selUser.Password = TxtPassword.Text;

                try
                {
                    UserDB.Update(selUser);
                    MessageBox.Show("Update successful");
                    RefreshForm();
                }
                catch
                {
                    MessageBox.Show("Error: Update not successful");
                }
            }
        }

        private bool FieldsAreValid()
        {
            if (CboRole.SelectedIndex < 0)
            {
                MessageBox.Show("Role is required.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(TxtFirstName.Text))
            {
                MessageBox.Show("First name is required.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(TxtLastName.Text))
            {
                MessageBox.Show("Last name is required.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(TxtUsername.Text))
            {
                MessageBox.Show("Username is required.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(TxtPassword.Text))
            {
                MessageBox.Show("Password is required.");
                return false;
            }

            return true;
        }

        private void FrmRegistration_Load(object sender, EventArgs e)
        {
            RefreshForm();
        }

        private void RefreshForm()
        {
            PopulateUserListBox();
            PopulateRoleComboBox();
            ClearAllFields();
        }

        private void PopulateRoleComboBox()
        {
            CboRole.DataSource = RoleDB.GetAllRoles();
        }

        private void PopulateUserListBox()
        {
            LstUser.DataSource = UserDB.GetAllUsers();
        }

        private void LstUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LstUser.SelectedIndex < 0)
            {
                BtnRegisterOrUpdate.Text = "Register";
                BtnDelete.Enabled = false;
                ClearAllFields();
            }
            else
            {
                BtnRegisterOrUpdate.Text = "Update";
                BtnDelete.Enabled = true;

                User selUser = (User)LstUser.SelectedItem;

                CboRole.SelectedItem = selUser.Role;
                TxtFirstName.Text = selUser.FirstName;
                TxtLastName.Text = selUser.LastName;
                TxtUsername.Text = selUser.Username;
                TxtPassword.Text = selUser.Password;
            }
        }

        private void ClearAllFields()
        {
            LstUser.SelectedIndex = -1;
            CboRole.SelectedIndex = -1;
            TxtFirstName.Text = string.Empty;
            TxtLastName.Text = string.Empty;
            TxtUsername.Text = string.Empty;
            TxtPassword.Text = string.Empty;
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            User selUser = (User)LstUser.SelectedItem;
            DialogResult result = MessageBox.Show($"Are you sure you want to delete {selUser}?", "Confirmation", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                try
                {
                    UserDB.Delete(selUser);
                    MessageBox.Show("User Deleted successfuly");
                    RefreshForm();
                }
                catch
                {
                    MessageBox.Show("Error: User not deleted successfuly");
                }
            }
                
        }
    }
}
