﻿using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

namespace App_Domain {

	/// <summary>
	/// Main window of the application
	/// </summary>
	public partial class Mainwin : Form {

		/// <summary>
		/// Used to set up columns in dgv when no transactions are loaded
		/// </summary>
		private DataTable EmptyTransactionTable;

		/// <summary>
		/// Default style of cells to left-align some where balances are right-aligned
		/// </summary>
		private DataGridViewCellStyle style;

		/// <summary>
		/// Used for printing
		/// </summary>
		private int currow;
		private int rowsleft;

		/// <summary>
		/// Constructor
		/// </summary>
		public Mainwin() { InitializeComponent(); }

		/// <summary>
		/// Initialize some stuff
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Mainwin_Load(object sender, EventArgs e) {
			style = new DataGridViewCellStyle();
			style.Alignment = DataGridViewContentAlignment.MiddleLeft;
			cbSortBy.SelectedIndex = 0;
			cbAccountType.SelectedIndex = 0;
			numAccountNumber.Maximum = decimal.MaxValue;
			lvJournalEntries.MultiSelect = false;
			tabMain.TabPages.Remove(tpAccountDetails);

			//Style datagridviews
			DataGridViewCellStyle cs = new DataGridViewCellStyle();
			cs.BackColor = Color.LightCyan;
			dgChartAccounts.AlternatingRowsDefaultCellStyle = cs;
			dgAccountTransactions.AlternatingRowsDefaultCellStyle = cs;
			dgAccountTypes.AlternatingRowsDefaultCellStyle = cs;
			dgChanges.AlternatingRowsDefaultCellStyle = cs;
			dgJournal.AlternatingRowsDefaultCellStyle = cs;
			dgTrialBalance.AlternatingRowsDefaultCellStyle = cs;
			dgJournalEntryTransactions.AlternatingRowsDefaultCellStyle = cs;
			dgIncomeSummary.AlternatingRowsDefaultCellStyle = cs;
			dgBalanceSheet.AlternatingRowsDefaultCellStyle = cs;
			//Add vertical scrollbar
			dgChartAccounts.ScrollBars = ScrollBars.Vertical;
			dgAccountTransactions.ScrollBars = ScrollBars.Vertical;
			dgAccountTypes.ScrollBars = ScrollBars.Vertical;
			dgChanges.ScrollBars = ScrollBars.Vertical;
			dgJournal.ScrollBars = ScrollBars.Vertical;
			dgTrialBalance.ScrollBars = ScrollBars.Vertical;
			dgIncomeSummary.ScrollBars = ScrollBars.Vertical;
			//setup unposted datagridview - need columns to be able to resize them
			EmptyTransactionTable = new DataTable();
			EmptyTransactionTable.Columns.Add("Account");
			EmptyTransactionTable.Columns.Add("Description");
			EmptyTransactionTable.Columns.Add("Debit");
			EmptyTransactionTable.Columns.Add("Credit");
			dgJournalEntryTransactions.DataSource = EmptyTransactionTable;

			cbAccountType.SelectedIndex = 0;
			//Cheated to make sure accounts show up by selecting types tab first
			tabMain.SelectTab(tpAllAccountTypes);
			tabMain.SelectTab(tpAllAccounts);

		}

		/// <summary>
		/// Refill datagridview when selecting a tab
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void tabMain_SelectedIndexChanged(object sender, EventArgs e) {
			if (tabMain.SelectedTab != tpAllJournalEntries) {
				btnPostJournalEntry.Enabled = false;
				btnDeleteJournalEntry.Enabled = false;
			}

			if (tabMain.SelectedTab == tpAllAccounts) {
				OnFillAccountCharts();
                mitemPrint.Enabled = true;
                mitemPreview.Enabled = true;
			} else if (tabMain.SelectedTab == tpAccountDetails) {
				//do nothing
                mitemPrint.Enabled = false;
                mitemPreview.Enabled = false;
			} else if (tabMain.SelectedTab == tpAllAccountTypes) {
				OnFillAccountTypes();
                mitemPrint.Enabled = false;
                mitemPreview.Enabled = false;
			} else if (tabMain.SelectedTab == tpAllTransactions) {
				OnFillTransactions();
                mitemPrint.Enabled = true;
                mitemPreview.Enabled = true;
			} else if (tabMain.SelectedTab == tpAllChanges) {
				OnFillAccountChanges();
                mitemPrint.Enabled = false;
                mitemPreview.Enabled = false;
			} else if (tabMain.SelectedTab == tpTrialBalance) {
				OnFillTrialBalance();
                mitemPrint.Enabled = true;
                mitemPreview.Enabled = true;
			} else if (tabMain.SelectedTab == tpAllJournalEntries) {
				OnFillJournalEntries();
                mitemPrint.Enabled = false;
                mitemPreview.Enabled = false;
			} else if (tabMain.SelectedTab == tpIncomeStatement) {
				OnFillIncomeSummary();
                mitemPrint.Enabled = true;
                mitemPreview.Enabled = true;
			} else if (tabMain.SelectedTab == tpBalanceSheet) {
				OnFillBalance();
                mitemPrint.Enabled = true;
                mitemPreview.Enabled = true;
			} else if (tabMain.SelectedTab == tpRetainedEarnings) {
				OnFillRE();
                mitemPrint.Enabled = false;
                mitemPreview.Enabled = false;
			} else if (tabMain.SelectedTab == tpRatios) {
				OnFillRatios();
                mitemPrint.Enabled = false;
                mitemPreview.Enabled = false;
			}
			resizeDataColumns();
		}

		/// <summary>
		/// Resize columns upon resizing
		/// </summary>
		private void resizeDataColumns() {
			if (tabMain.SelectedTab == tpAllAccounts) {
				dgChartAccounts.Columns[0].Width = 120;
				dgChartAccounts.Columns[2].Width = 80;
				dgChartAccounts.Columns[3].Width = 200;
				dgChartAccounts.Columns[1].Width = dgChartAccounts.Width - dgChartAccounts.Columns[0].Width - dgChartAccounts.Columns[2].Width - dgChartAccounts.Columns[3].Width - 20;
			} else if (tabMain.SelectedTab == tpAccountDetails) {
				dgAccountTransactions.Columns[0].Width = 80;
				dgAccountTransactions.Columns[2].Width = 120;
				dgAccountTransactions.Columns[3].Width = 120;
				dgAccountTransactions.Columns[1].Width = dgAccountTransactions.Width - dgAccountTransactions.Columns[3].Width - dgAccountTransactions.Columns[2].Width - dgAccountTransactions.Columns[0].Width;
			} else if (tabMain.SelectedTab == tpAllAccountTypes) {
				dgAccountTypes.Columns[0].Width = 160;
				dgAccountTypes.Columns[2].Width = 120;
				dgAccountTypes.Columns[1].Width = dgAccountTypes.Width - dgAccountTypes.Columns[0].Width - dgAccountTypes.Columns[2].Width - 20;
			} else if (tabMain.SelectedTab == tpAllTransactions) {
				dgJournal.Columns[0].Width = 80;
				dgJournal.Columns[1].Width = 80;
				dgJournal.Columns[3].Width = 120;
				dgJournal.Columns[4].Width = 120;
				dgJournal.Columns[5].Width = 150;
				dgJournal.Columns[2].Width = dgJournal.Width - dgJournal.Columns[0].Width - dgJournal.Columns[1].Width - dgJournal.Columns[3].Width - dgJournal.Columns[4].Width - dgJournal.Columns[5].Width - 20;
			} else if (tabMain.SelectedTab == tpAllChanges) {
				dgChanges.Columns[0].Width = 120;
				dgChanges.Columns[2].Width = 120;
				dgChanges.Columns[3].Width = 150;
				dgChanges.Columns[1].Width = dgChanges.Width - dgChanges.Columns[0].Width - dgChanges.Columns[2].Width - dgChanges.Columns[3].Width - 20;
			} else if (tabMain.SelectedTab == tpTrialBalance) {
				dgTrialBalance.Columns[0].Width = 120;
				dgTrialBalance.Columns[2].Width = 120;
				dgTrialBalance.Columns[3].Width = 120;
				dgTrialBalance.Columns[1].Width = dgTrialBalance.Width - dgTrialBalance.Columns[0].Width - dgTrialBalance.Columns[2].Width - dgTrialBalance.Columns[3].Width - 20;
				lblTBCompany.Left = tpTrialBalance.Width / 2 - lblTBCompany.Width / 2;
				lblTBTrialBalance.Left = tpTrialBalance.Width / 2 - lblTBTrialBalance.Width / 2;
				lblTBDate.Text = DateTime.Today.ToLongDateString();
				lblTBDate.Left = tpTrialBalance.Width / 2 - lblTBDate.Width / 2;
			} else if (tabMain.SelectedTab == tpAllJournalEntries) {
				dgJournalEntryTransactions.Columns[0].Width = 120;
				dgJournalEntryTransactions.Columns[2].Width = 100;
				dgJournalEntryTransactions.Columns[3].Width = 100;
				dgJournalEntryTransactions.Columns[1].Width = dgJournalEntryTransactions.Width - dgJournalEntryTransactions.Columns[0].Width - dgJournalEntryTransactions.Columns[2].Width - dgJournalEntryTransactions.Columns[3].Width;
			} else if (tabMain.SelectedTab == tpIncomeStatement) {
				dgIncomeSummary.Columns[0].Width = 100;
				dgIncomeSummary.Columns[2].Width = 100;
				dgIncomeSummary.Columns[1].Width = dgIncomeSummary.Width - dgIncomeSummary.Columns[0].Width - dgIncomeSummary.Columns[2].Width - 20;
				lblISCompany.Left = tpIncomeStatement.Width / 2 - lblISCompany.Width / 2;
				lblIIncomeStatement.Left = tpIncomeStatement.Width / 2 - lblIIncomeStatement.Width / 2;
				lblIDate.Text = DateTime.Today.ToLongDateString();
				lblIDate.Left = tpIncomeStatement.Width / 2 - lblIDate.Width / 2;
			} else if (tabMain.SelectedTab == tpBalanceSheet) {
				dgBalanceSheet.Columns[0].Width = 120;
				dgBalanceSheet.Columns[2].Width = 120;
				dgBalanceSheet.Columns[1].Width = dgBalanceSheet.Width - dgBalanceSheet.Columns[0].Width - dgBalanceSheet.Columns[2].Width - 20;
				lblBCompany.Left = tpBalanceSheet.Width / 2 - lblBCompany.Width / 2;
				lblBBalance.Left = tpBalanceSheet.Width / 2 - lblBBalance.Width / 2;
				lblBDate.Text = DateTime.Today.ToLongDateString();
				lblBDate.Left = tpBalanceSheet.Width / 2 - lblBDate.Width / 2;
			}
		}

		/// <summary>
		/// Close application
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Mainwin_FormClosed(object sender, FormClosedEventArgs e) {
			Application.Exit();
		}

		/// <summary>
		/// Refresh chart of accounts based upon filters
		/// </summary>
		public void OnFillAccountCharts() {
			if (cbxTypes.SelectedIndex > -1 && cbSortBy.SelectedIndex > -1) {
				bool care = false;
				bool act = false;
				int type = cbxTypes.SelectedIndex;
				if (!(cbSortBy.SelectedIndex == 0)) {
					care = true;//Active or Inactive selected
					act = cbSortBy.SelectedIndex == 1 ? true : false;//If active selected, true, otherwise false
				}
				dgChartAccounts.DataSource = Program.sqlcon.GetFilteredChartOfAccounts(care, act, type, txtAccountName.Text);
			}

			dgChartAccounts.ClearSelection();
		}

		/// <summary>
		/// Refresh account types
		/// </summary>
		public void OnFillAccountTypes() {
			dgAccountTypes.DataSource = Program.sqlcon.GetAccountTypes();
			dgAccountTypes.ClearSelection();

			//Populate account type dropdown on chart of accounts view
			cbxTypes.Items.Clear();
			cbxTypes.Items.Add("All");
			foreach (String each in Program.sqlcon.GetAccountTypesList())
				cbxTypes.Items.Add(each);
			cbxTypes.SelectedIndex = 0;
		}

		/// <summary>
		/// Refresh trial balance
		/// </summary>
		public void OnFillTrialBalance() {
			dgTrialBalance.DataSource = Program.sqlcon.GetTrialBalance();
			dgTrialBalance.Columns[0].DefaultCellStyle = style;
			dgTrialBalance.Columns[1].DefaultCellStyle = style;
		}

		/// <summary>
		/// Refresh balance sheet
		/// </summary>
		public void OnFillBalance() {
			dgBalanceSheet.DataSource = Program.sqlcon.GetBalanceSheet();
			//Left align some columns
			dgBalanceSheet.Columns[0].DefaultCellStyle = style;
			dgBalanceSheet.Columns[1].DefaultCellStyle = style;
		}

		/// <summary>
		/// Refresh ratio reports
		/// </summary>
		public void OnFillRatios() {
			double assests = Program.sqlcon.GetAccountTypeTotal(0);
			double liabilities = Program.sqlcon.GetAccountTypeTotal(1);
			//double equity = Program.sqlcon.GetAccountTypeTotal(4);
			//double revenue = Program.sqlcon.GetRevenues();
			//double expenses = Program.sqlcon.GetExpenses();
			double quickassets = Program.sqlcon.GetQuickAssetBalance();
			double ratio;

			if (liabilities != 0) {
				ratio = Math.Round(Math.Abs(assests / liabilities), 2);
				txtCurRatio.Text = ratio.ToString();

				ratio = Math.Round(Math.Abs(quickassets / liabilities), 2);
				txtQuickRatio.Text = ratio.ToString();
			} else {
				txtCurRatio.Text = "Zero liabilities";
				txtQuickRatio.Text = "Zero liabilities";
			}

		}

		/// <summary>
		/// Refresh journal entries
		/// </summary>
		public void OnFillJournalEntries() {
			lvJournalEntries.Items.Clear();
			DataTable dt = Program.sqlcon.GetUnpostedJournalEntries();
			tpAllJournalEntries.Text = "Journal Entries";
			if (dt != null) {
				if (dt.Rows.Count > 0) {
					tpAllJournalEntries.Text += " (" + dt.Rows.Count + ")";//Show # of unposted entries
					btnPostAllJournalEntries.Enabled = true;
				}

				foreach (DataRow each in dt.Rows) {
					ListViewItem item = new ListViewItem(new string[] { each["id"].ToString(), each["postdate"].ToString() }, lvJournalEntries.Groups["Unposted"]);
					item.Name = each["id"].ToString();//Necessary to look up Journal Entry by key later
					lvJournalEntries.Items.Add(item);
				}
			}
			dt = null;
			dt = Program.sqlcon.GetDeletedJournalEntries();
			if (dt != null) {
				foreach (DataRow each in dt.Rows) {
					ListViewItem item = new ListViewItem(new string[] { each["id"].ToString(), each["postdate"].ToString() }, lvJournalEntries.Groups["Deleted"]);
					item.Name = each["id"].ToString();//Necessary to look up Journal Entry by key later
					lvJournalEntries.Items.Add(item);
				}
			}
			dt = null;
			dt = Program.sqlcon.GetPostedJournalEntries();
			if (dt != null) {
				foreach (DataRow each in dt.Rows) {
					ListViewItem item = new ListViewItem(new string[] { each["id"].ToString(), each["postdate"].ToString() }, lvJournalEntries.Groups["Posted"]);
					item.Name = each["id"].ToString();//Necessary to look up Journal Entry by key later
					lvJournalEntries.Items.Add(item);
				}
			}

			if (lvJournalEntries.Groups["Unposted"].Items.Count == 0)
				btnPostAllJournalEntries.Enabled = false;
			else
				btnPostAllJournalEntries.Enabled = true;
		}

		/// <summary>
		/// Refresh income statement
		/// </summary>
		public void OnFillIncomeSummary() {
			dgIncomeSummary.DataSource = Program.sqlcon.GetIncomeStatement();
			dgIncomeSummary.Columns[0].DefaultCellStyle = style;
			dgIncomeSummary.Columns[1].DefaultCellStyle = style;
		}

		/// <summary>
		/// Refresh income, dividends, and retained earnings
		/// </summary>
		public void OnFillRE() {
			double income = Program.sqlcon.GetRevenues() - Program.sqlcon.GetExpenses();
			txtIncome.Text = income.ToString();
			numDividends.Maximum = income > 0 ? (decimal) income : 0 ;
			numDividends_ValueChanged(this, new EventArgs());
		}

		/// <summary>
		/// Refresh account changes dgv
		/// </summary>
		public void OnFillAccountChanges() {
			dgChanges.DataSource = Program.sqlcon.GetAccountChanges();
			dgChanges.ClearSelection();
		}

		/// <summary>
		/// Refresh journal as well as changes, trial balance, and journal entries
		/// </summary>
		public void OnFillTransactions() {
			dgJournal.DataSource = Program.sqlcon.GetJournal();
			dgJournal.ClearSelection();
			dgJournal.Columns[0].DefaultCellStyle = style;
			dgJournal.Columns[1].DefaultCellStyle = style;
			dgJournal.Columns[2].DefaultCellStyle = style;
			dgJournal.Columns[5].DefaultCellStyle = style;
		}

		/// <summary>
		/// Show a specific accounts' transaction in the info tab
		/// </summary>
		/// <param name="accountnum"></param>
		private void OnFillAccountTransactions(int accountnum) {
			lblBalance.Text = String.Format("Balance: {0:C}", Program.sqlcon.GetAccountBalance(accountnum));
			dgAccountTransactions.DataSource = Program.sqlcon.GetAccountLedger(accountnum);
			dgAccountTransactions.ClearSelection();
			if (dgAccountTransactions.Columns.Count > 0) {
				dgAccountTransactions.Columns[0].DefaultCellStyle = style;
				dgAccountTransactions.Columns[1].DefaultCellStyle = style;
			}
		}

		/// <summary>
		/// Filter chart of accounts by active status
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ChartOfAccounts_FilterChanged(object sender, EventArgs e) { OnFillAccountCharts(); }

		/// <summary>
		/// Select an account to view information
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void dgChartAccounts_DoubleClick(object sender, EventArgs e) {
			if (dgChartAccounts.SelectedRows.Count > 0) {
				DataTable dt = Program.sqlcon.GetAccountByNumber(Convert.ToInt32(dgChartAccounts.SelectedRows[0].Cells[0].Value.ToString()));

				if (dt != null && dt.Rows.Count > 0) {
					//Show tab
					if (!tabMain.TabPages.Contains(tpAccountDetails)) {
						tabMain.TabPages.Insert(1, tpAccountDetails);
					}
					//Show name in groupbox
					gbAccount.Text = dt.Rows[0]["accountnum"] + " - " + dt.Rows[0]["descript"];
					//Show account's transactions
					OnFillAccountTransactions(Convert.ToInt32(dt.Rows[0]["accountnum"]));
					//Show active status and only allow changes if balance is 0
					int active = Convert.ToInt32(dt.Rows[0]["active"]);
					cbAccountActive.CheckedChanged -= cbAccountActive_CheckedChanged;//So loading the state is added as a change
					cbAccountActive.Checked = active == 1 ? true : false;//If active == 1, check the txtAccounts
					cbAccountActive.CheckedChanged += cbAccountActive_CheckedChanged;//So changes to it will be added as a change
					numAccountNumber.Value = Convert.ToDecimal(dt.Rows[0]["accountnum"]);
					if (Program.sqlcon.GetAccountBalance(Convert.ToInt32(dt.Rows[0]["accountnum"])) != 0)
						cbAccountActive.Enabled = false;
					else
						cbAccountActive.Enabled = true;
					DataTable isEmpty = Program.sqlcon.GetAccountLedger(Convert.ToInt32(dt.Rows[0]["accountnum"]));
					if (isEmpty != null && isEmpty.Rows.Count < 1)
						btnDeleteAccount.Visible = true;
					else
						btnDeleteAccount.Visible = false;
					tabMain.SelectedTab = tpAccountDetails;
				}
				dgChartAccounts.ClearSelection();
			}
		}

		/// <summary>
		/// Draw tabs
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void tabMain_DrawItem(object sender, DrawItemEventArgs e) {
			Graphics g = e.Graphics;
			Brush _textBrush;

			// Get the item from the collection.
			TabPage _tabPage = tabMain.TabPages[e.Index];

			// Get the real bounds for the tab rectangle.
			Rectangle _tabBounds = tabMain.GetTabRect(e.Index);

			// Get font
			Font _tabFont = _tabPage.Font;


			if (e.State == DrawItemState.Selected) {
				// Draw a different background color, and don't paint a focus rectangle.
				_textBrush = new SolidBrush(Color.White);
				g.FillRectangle(Brushes.Gray, e.Bounds);
			} else {
				_textBrush = new System.Drawing.SolidBrush(e.ForeColor);
				e.DrawBackground();
			}

			// Draw string. Center the text.
			StringFormat _stringFlags = new StringFormat();
			_stringFlags.Alignment = StringAlignment.Center;
			_stringFlags.LineAlignment = StringAlignment.Center;
			g.DrawString(_tabPage.Text, _tabFont, _textBrush, _tabBounds, new StringFormat(_stringFlags));
		}

		/// <summary>
		/// Change active status of an account
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cbAccountActive_CheckedChanged(object sender, EventArgs e) {
			int i = Convert.ToInt32(gbAccount.Text.IndexOf(" "));
			Program.sqlcon.ChangeAccountStatusByNumber(Convert.ToInt32(gbAccount.Text.Substring(0, i)), cbAccountActive.Checked);
			OnFillAccountCharts();
		}

		/// <summary>
		/// Enable delete/post buttons as needed when different journal entries are selected. Load JE data into tab
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void lvJournalEntries_SelectedIndexChanged(object sender, EventArgs e) {
			//Make sure something selected
			if (lvJournalEntries.SelectedItems.Count > 0) {
				//Pull info about journal entry into datagridview and textbox
				int refnum = Convert.ToInt32(lvJournalEntries.SelectedItems[0].Text);
				dgJournalEntryTransactions.DataSource = Program.sqlcon.GetJournalEntryTransactions(refnum);
				txtNotes.Text = Program.sqlcon.GetJournalEntryNote(refnum);
				//enable buttons as needed
				dgJournalEntryTransactions.Sort(dgJournalEntryTransactions.Columns[2], System.ComponentModel.ListSortDirection.Descending);
				if (lvJournalEntries.SelectedItems[0].Group == lvJournalEntries.Groups["Unposted"]) {
					btnPostJournalEntry.Enabled = true;
					btnDeleteJournalEntry.Enabled = true;
				} else if (lvJournalEntries.SelectedItems[0].Group == lvJournalEntries.Groups["Posted"]) {
					btnPostJournalEntry.Enabled = false;
					btnDeleteJournalEntry.Enabled = false;
				} else if (lvJournalEntries.SelectedItems[0].Group == lvJournalEntries.Groups["Deleted"]) {
					btnPostJournalEntry.Enabled = true;
					btnDeleteJournalEntry.Enabled = false;
				}
			}
		}

		/// <summary>
		/// Post selected journal entry
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnPostJournalEntry_Click(object sender, EventArgs e) {
			//make sure something is selected still
			if (lvJournalEntries.SelectedItems.Count > 0) {
				int refnum = Convert.ToInt32(lvJournalEntries.SelectedItems[0].Text);
				Program.sqlcon.PostJournalEntry(refnum);
				btnPostJournalEntry.Enabled = false;
				btnDeleteJournalEntry.Enabled = false;
				//Refresh
				OnFillJournalEntries();
				dgJournalEntryTransactions.DataSource = EmptyTransactionTable;
			}
		}

		/// <summary>
		/// Delete selected journal entry
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnDeleteJournalEntry_Click(object sender, EventArgs e) {
			//make sure something is selected still
			if (lvJournalEntries.SelectedItems.Count > 0) {
				int refnum = Convert.ToInt32(lvJournalEntries.SelectedItems[0].Text);
				Program.sqlcon.DeleteJournalEntry(refnum);
				btnPostJournalEntry.Enabled = false;
				btnDeleteJournalEntry.Enabled = false;
				//Refresh
				OnFillJournalEntries();
				dgJournalEntryTransactions.DataSource = EmptyTransactionTable;
			}
		}

		/// <summary>
		/// Post all unposted journal entries that are not deleted
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnPostAllJournalEntries_Click(object sender, EventArgs e) {
			foreach (ListViewItem each in lvJournalEntries.Groups["Unposted"].Items) {
				Program.sqlcon.PostJournalEntry(Convert.ToInt32(each.Text));
			}
			btnPostAllJournalEntries.Enabled = false;
			btnDeleteAccount.Enabled = false;
			btnPostJournalEntry.Enabled = false;
			//Refresh
			OnFillJournalEntries();
			dgJournalEntryTransactions.DataSource = EmptyTransactionTable;
		}

		/// <summary>
		/// Add journal entry
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnAddJournalEntry_Click(object sender, EventArgs e) {
			new frmAddJournalEntry().ShowDialog();
			tabMain.SelectTab(tabMain.SelectedTab);
			OnFillJournalEntries();
		}

		/// <summary>
		/// Create a new account type
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnAddAccountType_Click(object sender, EventArgs e) {
			if (txtAccountTypeName.Text != "" && txtAccountTypeDescription.Text != "") {
				DataTable dt = Program.sqlcon.GetAccountTypeByName(txtAccountTypeName.Text);
				if (dt != null && dt.Rows.Count < 1) {//Check for existing type of that name
					//Account type type now dictates whether debit is positive or not
					bool debitIsPositive = cbAccountType.SelectedItem.ToString().Equals("Asset") || cbAccountType.SelectedItem.ToString().Equals("Expense") ? true : false;
					Program.sqlcon.AddAccountType(txtAccountTypeName.Text, txtAccountTypeDescription.Text, debitIsPositive, cbAccountType.SelectedIndex);
					OnFillAccountTypes();
				} else {
					MessageBox.Show("This account type already exists. Please enter something else.");
				}
			} else
				MessageBox.Show("Please enter a name and description for this new account type.");
		}

		/// <summary>
		/// Delete an account (only available when account has no transactions
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnDeleteAccount_Click(object sender, EventArgs e) {
			//Delete Account
			int i = Convert.ToInt32(gbAccount.Text.IndexOf(" "));
			Program.sqlcon.DeleteAccount(Convert.ToInt32(gbAccount.Text.Substring(0, i)));
			OnFillAccountCharts();
			//Go back to main and hide tab
			tabMain.SelectTab(tpAllAccounts);
			tabMain.TabPages.Remove(tpAccountDetails);
		}

		/// <summary>
		/// Update displayed numbers for dividends and retained earnings
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void numDividends_ValueChanged(object sender, EventArgs e) {
			double income = Convert.ToDouble(txtIncome.Text);
			double dividends = (double)numDividends.Value;
			txtRE.Text = (income - dividends).ToString();
		}

		/// <summary>
		/// Update displayed numbers for dividends and retained earnings
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void numDividends_Leave(object sender, EventArgs e) {
			numDividends_ValueChanged(this, new EventArgs());
		}

		/// <summary>
		/// Save dividends
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSaveRE_Click(object sender, EventArgs e) {
			//Program.sqlcon.SetRE(Convert.ToDouble(txtRE.Text));
			Program.sqlcon.SetDividends((double)numDividends.Value);
		}

		/// <summary>
		/// Double-clicking a transaction pulls up the relevant journal entry
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void dgAccountTransactions_DoubleClick(object sender, EventArgs e) {
			if (dgAccountTransactions.SelectedRows.Count > 0) {
				string refnum = dgAccountTransactions.SelectedRows[0].Cells[0].Value.ToString();
				tabMain.SelectTab(tpAllJournalEntries);
				if (lvJournalEntries.Items[refnum] != null) {
					lvJournalEntries.Items[refnum].Selected = true;
					lvJournalEntries.Items[refnum].EnsureVisible();
				}
				lvJournalEntries.Select();
			}
		}

		/// <summary>
		/// Double-clicking a transaction pulls up the relevant journal entry
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void dgJournal_DoubleClick(object sender, EventArgs e) {
			if (dgJournal.SelectedRows.Count > 0) {
				string refnum = dgJournal.SelectedRows[0].Cells[0].Value.ToString();
				tabMain.SelectTab(tpAllJournalEntries);
				if (lvJournalEntries.Items[refnum] != null) {
					lvJournalEntries.Items[refnum].Selected = true;
					lvJournalEntries.Items[refnum].EnsureVisible();
				}
				lvJournalEntries.Select();
			}
		}

		/// <summary>
		/// Try to update the number of an account
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnUpdateAccountNumber_Click(object sender, EventArgs e) {
			//Check the number's not taken
			if (Program.sqlcon.GetAccountByNumber(Convert.ToInt32(numAccountNumber.Value)).Rows.Count < 1) {
				int i = Convert.ToInt32(gbAccount.Text.IndexOf(" "));
				Program.sqlcon.ChangeAccountNumberByNumber(Convert.ToInt32(gbAccount.Text.Substring(0, i)), Convert.ToInt32(numAccountNumber.Value));
				gbAccount.Text = numAccountNumber.Value.ToString() + gbAccount.Text.Substring(i);
			} else {
				MessageBox.Show("This number is already taken. Please try another.");
			}
		}

		/// <summary>
		/// Resize colummns when window changes size
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Mainwin_Resize(object sender, EventArgs e) {
			resizeDataColumns();
		}

		/// <summary>
		/// Create a new account in new form
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnNewAccount_Click(object sender, EventArgs e) {
			new AddAccount().ShowDialog();
			tabMain.SelectTab(tabMain.SelectedTab);
		}

		/// <summary>
		/// Open up print preview
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnPrint_Click(object sender, EventArgs e) {
			ppPrinterPreview.Document = pdChartOfAccounts;
			ppPrinterPreview.ShowDialog();
		}

		/// <summary>
		/// Set up document to print
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void pdPrinterDoc_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e) {
			Pen BlackPen = new Pen(Color.Black);
			SolidBrush BlackBrush = new SolidBrush(Color.Black);

			Font pt12 = new Font(FontFamily.GenericSerif, 11);
			Font pt18B = new Font(FontFamily.GenericSerif, 18);

			int lineY = 100;
			int textY = 120;
			int counter;

			DataTable dt = Program.sqlcon.GetPrintTable();

			if (rowsleft > 43) {
				counter = 44;
				rowsleft -= 44;
				e.HasMorePages = true;
			} else {
				counter = dt.Rows.Count - currow;
				e.HasMorePages = false;
			}

            e.Graphics.DrawString("Chart of Accounts", pt18B, BlackBrush, new Point(350, 60));
            e.Graphics.DrawString("Account", pt12, BlackBrush, new Point(80, 100));
            e.Graphics.DrawString("Balance", pt12, BlackBrush, new Point(650, 100));

			for (int i = 0; i < counter; i++) {
				e.Graphics.DrawString(dt.Rows[currow][0].ToString(), pt12, BlackBrush, new Point(80, textY));
				e.Graphics.DrawString(dt.Rows[currow][1].ToString(), pt12, BlackBrush, new Point(120, textY));
				e.Graphics.DrawString(String.Format("{0:C}", dt.Rows[currow][2]), pt12, BlackBrush, new Point(650, textY));
				//e.Graphics.DrawString(dt.Rows[currow][3].ToString(), pt12, BlackBrush, new Point(550, textY));
				textY += 20;
				currow++;
			}

			while (lineY <= 1000) {
				e.Graphics.DrawLine(BlackPen, new Point(75, lineY), new Point(775, lineY));
				lineY += 20;
			}
			e.Graphics.DrawLine(BlackPen, new Point(75, 100), new Point(75, 1000));
			e.Graphics.DrawLine(BlackPen, new Point(775, 100), new Point(775, 1000));

		}

		/// <summary>
		/// Start printing
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void pdPrinterDoc_BeginPrint(object sender, System.Drawing.Printing.PrintEventArgs e) {
			rowsleft = Program.sqlcon.GetPrintTable().Rows.Count;
			currow = 0;
		}

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabMain.SelectedTab == tpAllAccounts)
            {
                pdialogPrinter.Document = pdChartOfAccounts;
                pdialogPrinter.ShowDialog();
            }
            else if (tabMain.SelectedTab == tpAllTransactions)
            {
                pdialogPrinter.Document = pdJournaL;
                pdialogPrinter.ShowDialog();
            }
            else if (tabMain.SelectedTab == tpTrialBalance)
            {
                pdialogPrinter.Document = pdTrialBalance;
                pdialogPrinter.ShowDialog();
            }
            else if (tabMain.SelectedTab == tpBalanceSheet)
            {
                pdialogPrinter.Document = pdBalanceSheet;
                pdialogPrinter.ShowDialog();
            }
            else if (tabMain.SelectedTab == tpIncomeStatement)
            {
                pdialogPrinter.Document = pdIncome;
                pdialogPrinter.ShowDialog();
            }
        }

        private void mitemExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void priToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabMain.SelectedTab == tpAllAccounts)
            {
                ppPrinterPreview.Document = pdChartOfAccounts;
                ppPrinterPreview.ShowDialog();
            }
            else if (tabMain.SelectedTab == tpAllTransactions)
            {
                ppPrinterPreview.Document = pdJournaL;
                ppPrinterPreview.ShowDialog();
            }
            else if (tabMain.SelectedTab == tpTrialBalance)
            {
                ppPrinterPreview.Document = pdTrialBalance;
                ppPrinterPreview.ShowDialog();
            }
            else if (tabMain.SelectedTab == tpBalanceSheet)
            {
                ppPrinterPreview.Document = pdBalanceSheet;
                ppPrinterPreview.ShowDialog();
            }
            else if (tabMain.SelectedTab == tpIncomeStatement)
            {
                ppPrinterPreview.Document = pdIncome;
                ppPrinterPreview.ShowDialog();
            }
        }

        private void pdTrialBalance_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Pen BlackPen = new Pen(Color.Black);
            SolidBrush BlackBrush = new SolidBrush(Color.Black);

            Font pt12 = new Font(FontFamily.GenericSerif, 11);
            Font pt18B = new Font(FontFamily.GenericSerif, 18);

            int lineY = 100;
            int textY = 120;
            int counter;

            DataTable dt = Program.sqlcon.GetTrialBalance();

            if (rowsleft > 43)
            {
                counter = 44;
                rowsleft -= 44;
                e.HasMorePages = true;
            }
            else
            {
                counter = dt.Rows.Count - currow;
                e.HasMorePages = false;
            }

            e.Graphics.DrawString("Trial Balance", pt18B, BlackBrush, new Point(350, 60));
            e.Graphics.DrawString("Account", pt12, BlackBrush, new Point(80, 100));
            e.Graphics.DrawString("Debits", pt12, BlackBrush, new Point(500, 100));
            e.Graphics.DrawString("Credits", pt12, BlackBrush, new Point(625, 100));

            for (int i = 0; i < counter; i++)
            {
                e.Graphics.DrawString(dt.Rows[currow][0].ToString(), pt12, BlackBrush, new Point(80, textY));
                e.Graphics.DrawString(dt.Rows[currow][1].ToString(), pt12, BlackBrush, new Point(120, textY));
                e.Graphics.DrawString(String.Format("{0:C}", dt.Rows[currow][2]), pt12, BlackBrush, new Point(500, textY));
                e.Graphics.DrawString(String.Format("{0:C}", dt.Rows[currow][3]), pt12, BlackBrush, new Point(625, textY));
                textY += 20;
                currow++;
            }

            while (lineY <= 1000)
            {
                e.Graphics.DrawLine(BlackPen, new Point(75, lineY), new Point(775, lineY));
                lineY += 20;
            }
            e.Graphics.DrawLine(BlackPen, new Point(75, 100), new Point(75, 1000));
            e.Graphics.DrawLine(BlackPen, new Point(775, 100), new Point(775, 1000));
        }

        private void pdTrialBalance_BeginPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            rowsleft = Program.sqlcon.GetTrialBalance().Rows.Count;
            currow = 0;
        }

        private void pdBalanceSheet_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Pen BlackPen = new Pen(Color.Black);
            SolidBrush BlackBrush = new SolidBrush(Color.Black);

            Font pt12 = new Font(FontFamily.GenericSerif, 11);
            Font pt18B = new Font(FontFamily.GenericSerif, 18);

            int lineY = 100;
            int textY = 120;
            int counter;

            DataTable dt = Program.sqlcon.GetBalanceSheet();

            if (rowsleft > 43)
            {
                counter = 44;
                rowsleft -= 44;
                e.HasMorePages = true;
            }
            else
            {
                counter = dt.Rows.Count - currow;
                e.HasMorePages = false;
            }

            e.Graphics.DrawString("Balance Sheet", pt18B, BlackBrush, new Point(350, 60));
            e.Graphics.DrawString("Account", pt12, BlackBrush, new Point(80, 100));
            e.Graphics.DrawString("Balance", pt12, BlackBrush, new Point(600, 100));

            for (int i = 0; i < counter; i++)
            {
                e.Graphics.DrawString(dt.Rows[currow][0].ToString(), pt12, BlackBrush, new Point(80, textY));
                e.Graphics.DrawString(dt.Rows[currow][1].ToString(), pt12, BlackBrush, new Point(120, textY));
                e.Graphics.DrawString(String.Format("{0:C}", dt.Rows[currow][2]), pt12, BlackBrush, new Point(600, textY));
                //e.Graphics.DrawString(String.Format("{0:C}", dt.Rows[currow][3]), pt12, BlackBrush, new Point(625, textY));
                textY += 20;
                currow++;
            }

            while (lineY <= 1000)
            {
                e.Graphics.DrawLine(BlackPen, new Point(75, lineY), new Point(775, lineY));
                lineY += 20;
            }
            e.Graphics.DrawLine(BlackPen, new Point(75, 100), new Point(75, 1000));
            e.Graphics.DrawLine(BlackPen, new Point(775, 100), new Point(775, 1000));
        }

        private void pdBalanceSheet_BeginPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            rowsleft = Program.sqlcon.GetBalanceSheet().Rows.Count;
            currow = 0;
        }

        private void pdIncome_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Pen BlackPen = new Pen(Color.Black);
            SolidBrush BlackBrush = new SolidBrush(Color.Black);

            Font pt12 = new Font(FontFamily.GenericSerif, 11);
            Font pt18B = new Font(FontFamily.GenericSerif, 18);

            int lineY = 100;
            int textY = 120;
            int counter;

            DataTable dt = Program.sqlcon.GetIncomeStatement();

            if (rowsleft > 43)
            {
                counter = 44;
                rowsleft -= 44;
                e.HasMorePages = true;
            }
            else
            {
                counter = dt.Rows.Count - currow;
                e.HasMorePages = false;
            }

            e.Graphics.DrawString("Income Statement", pt18B, BlackBrush, new Point(350, 60));
            e.Graphics.DrawString("Account", pt12, BlackBrush, new Point(80, 100));
            e.Graphics.DrawString("Ammount", pt12, BlackBrush, new Point(600, 100));

            for (int i = 0; i < counter; i++)
            {
                e.Graphics.DrawString(dt.Rows[currow][0].ToString(), pt12, BlackBrush, new Point(80, textY));
                e.Graphics.DrawString(dt.Rows[currow][1].ToString(), pt12, BlackBrush, new Point(120, textY));
                e.Graphics.DrawString(String.Format("{0:C}", dt.Rows[currow][2]), pt12, BlackBrush, new Point(600, textY));
                //e.Graphics.DrawString(String.Format("{0:C}", dt.Rows[currow][3]), pt12, BlackBrush, new Point(625, textY));
                textY += 20;
                currow++;
            }

            while (lineY <= 1000)
            {
                e.Graphics.DrawLine(BlackPen, new Point(75, lineY), new Point(775, lineY));
                lineY += 20;
            }
            e.Graphics.DrawLine(BlackPen, new Point(75, 100), new Point(75, 1000));
            e.Graphics.DrawLine(BlackPen, new Point(775, 100), new Point(775, 1000));
        }

        private void pdIncome_BeginPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            rowsleft = Program.sqlcon.GetIncomeStatement().Rows.Count;
            currow = 0;
        }

        private void pdJournaL_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Pen BlackPen = new Pen(Color.Black);
            SolidBrush BlackBrush = new SolidBrush(Color.Black);

            Font pt12 = new Font(FontFamily.GenericSerif, 11);
            Font pt18B = new Font(FontFamily.GenericSerif, 18);

            int lineY = 100;
            int textY = 120;
            int counter;

            DataTable dt = Program.sqlcon.GetJournal();

            if (rowsleft > 43)
            {
                counter = 44;
                rowsleft -= 44;
                e.HasMorePages = true;
            }
            else
            {
                counter = dt.Rows.Count - currow;
                e.HasMorePages = false;
            }

            e.Graphics.DrawString("Journal Entries", pt18B, BlackBrush, new Point(350, 60));
            e.Graphics.DrawString("Account", pt12, BlackBrush, new Point(80, 100));
            e.Graphics.DrawString("Debits", pt12, BlackBrush, new Point(400, 100));
            e.Graphics.DrawString("Credits", pt12, BlackBrush, new Point(510, 100));
            e.Graphics.DrawString("Date", pt12, BlackBrush, new Point(610, 100));

            for (int i = 0; i < counter; i++)
            {
                e.Graphics.DrawString(dt.Rows[currow][1].ToString(), pt12, BlackBrush, new Point(80, textY));
                e.Graphics.DrawString(dt.Rows[currow][2].ToString(), pt12, BlackBrush, new Point(120, textY));
                e.Graphics.DrawString(String.Format("{0:C}", dt.Rows[currow][3]), pt12, BlackBrush, new Point(400, textY));
                e.Graphics.DrawString(String.Format("{0:C}", dt.Rows[currow][4]), pt12, BlackBrush, new Point(510, textY));
                e.Graphics.DrawString(dt.Rows[currow][5].ToString(), pt12, BlackBrush, new Point(610, textY));
                textY += 20;
                currow++;
            }

            while (lineY <= 1000)
            {
                e.Graphics.DrawLine(BlackPen, new Point(75, lineY), new Point(775, lineY));
                lineY += 20;
            }
            e.Graphics.DrawLine(BlackPen, new Point(75, 100), new Point(75, 1000));
            e.Graphics.DrawLine(BlackPen, new Point(775, 100), new Point(775, 1000));
        }

        private void pdJournaL_BeginPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            rowsleft = Program.sqlcon.GetJournal().Rows.Count;
            currow = 0;
        }

	}//end Mainwin class
}//end namespace
