using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.Data;
using DevExpress.XtraBars;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using EntityUI.Controls;

namespace EntityUI
{
    public partial class ReestrFormBase<T> : ReestrFormBase where T : class
    {
        private readonly IEntityProvider _entityProvider;
        private readonly Engine _engine;

        public ReestrFormBase()
        {
            InitializeComponent();
            SaveButton.Visibility = BarItemVisibility.Never;
            Load += OnLoad;
        }

        private readonly Dictionary<string,Func<T, object>> _unboundColumnFunctions = new Dictionary<string, Func<T, object>>();

        public ReestrFormBase(IEntityProvider entityProvider, Engine engine)
            : this()
        {
            _entityProvider = entityProvider;
            _engine = engine;

            var source = new BindingSource { DataSource = Items};
            gridControl1.DataSource = source;

            gridView1.CustomUnboundColumnData += GridView1OnCustomUnboundColumnData;
            gridView1.BeginUpdate();
            foreach (var prop in typeof(T).GetProperties().Where(x=> typeof(IEnumerable).IsAssignableFrom(x.PropertyType) && x.PropertyType.IsGenericType))
            {
                gridView1.Columns.Add(new GridColumn
                {
                    Name = prop.Name,
                    FieldName = prop.Name,
                    Visible = true,
                    UnboundType = UnboundColumnType.String
                });

                _unboundColumnFunctions[prop.Name] = item => prop.GetValue(item);
            }
            gridView1.EndUpdate();
        }

        private void GridView1OnCustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            if (_unboundColumnFunctions.ContainsKey(e.Column.FieldName))
            {
                var item = Items[e.ListSourceRowIndex];
                var propValue = _unboundColumnFunctions[e.Column.FieldName](item);

                e.Value = string.Join(",", (propValue as IEnumerable).OfType<object>());
            }
        }

        public List<T> Items { get; } = new List<T>();

        public List<T> SelectedItems
        {
            get
            {
                var rows = gridView1.GetSelectedRows();
                
                return rows
                    .Select(x => Items[gridView1.GetDataSourceRowIndex(x)])
                    .ToList();
            }
        }

        private async void OnLoad(object sender, EventArgs eventArgs)
        {
            await LoadDataSource();
        }

        private async Task LoadDataSource()
        {
            var list = await Task.Run(() => _entityProvider.GetList<T>());
            Items.Clear();
            Items.AddRange(list);

            gridControl1.RefreshDataSource();
        }

        private void gridControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var hitInfo = gridView1.CalcHitInfo(e.Location);
                if (hitInfo.InRowCell)
                    popupMenu1.ShowPopup(MousePosition);
            }
        }

        private async void barButtonItemAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            using (var form = _engine.CreateCardForm(default(T)))
            {
                form.ShowDialog(this);
            }

            await LoadDataSource();
        }

        private async void barButtonItemEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var rows = gridView1.GetSelectedRows();
            if (rows.Length == 0)
                return;

            var row = gridView1.GetDataSourceRowIndex(rows[0]);

            using (var form = _engine.CreateCardForm(Items[row]))
            {
                form.ShowDialog(this);
            }

            await LoadDataSource();
        }

        private async void barButtonItemDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var rows = gridView1.GetSelectedRows();
            if (rows.Length == 0)
                return;

            foreach (var rowIndex in rows)
            {
                var row = gridView1.GetDataSourceRowIndex(rowIndex);

                await Task.Run(() => _entityProvider.Delete(Items[row]));
            }

            await LoadDataSource();
        }

        private void barButtonItemSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        public BarButtonItem SaveButton => barButtonItemSave;
        public GridView GridView => gridView1;

        private async void barButtonItemRefresh_ItemClick(object sender, ItemClickEventArgs e)
        {
            await LoadDataSource();
        }
    }
}
