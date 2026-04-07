using IPB2.OnlineBusSystem.Domain.Common;
using IPB2.OnlineBusSystem.Domain.Features.Bus;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IPB2.OnlineBusSystem.WindowFormApp.Featues.Admin
{
    public partial class BusNewForm : Form
    {
        private readonly IBusService _busService;

        public BusNewForm(IBusService busService)
        {
            _busService = busService;
            InitializeComponent();
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBusNo.Text) || string.IsNullOrWhiteSpace(txtBusName.Text))
            {
                MessageBox.Show("Please fill all required fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtTotalSeat.Text, out int totalSeat))
            {
                MessageBox.Show("Total Seat must be a number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (totalSeat  < 20)
            {
                MessageBox.Show("Total Seat must be at leave 20 seats.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var request = new UpsertBusRequest
            {
                BusNo = txtBusNo.Text,
                BusName = txtBusName.Text,
                BusType = txtBusType.Text,
                TotalSeat = totalSeat
            };

            var response = await _busService.CreateAsync(request);

            if (response.Status == IPB2.OnlineBusSystem.Domain.Common.ResponseType.Success)
            {
                MessageBox.Show(response.Message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show(response.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
