using IPB2.OnlineBusSystem.Domain.Features.Route;
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
    public partial class RouteNewForm : Form
    {
        private readonly IRouteService _routeService;

        public RouteNewForm(IRouteService routeService)
        {
            _routeService = routeService;
            InitializeComponent();
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRouteName.Text) || string.IsNullOrWhiteSpace(txtOrigin.Text) || string.IsNullOrWhiteSpace(txtDestination.Text))
            {
                MessageBox.Show("Please fill all required fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var request = new UpsertRouteRequest
            {
                RouteName = txtRouteName.Text,
                Origin = txtOrigin.Text,
                Destination = txtDestination.Text
            };

            var response = await _routeService.CreateAsync(request);

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
