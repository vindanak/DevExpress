using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.Diagram;
using DevExpress.Xpf.Grid;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using static Layout.ContainerViewModel;

namespace Layout
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : UserControl
    {
        private static int _oneFtToPxel = 37;
        private static double _tablerowheight = 26.5;
        string xmlContent = string.Empty;
        ContainerViewModel containerViewModel = new ContainerViewModel();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void bbi_Open_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(xmlContent);
            MemoryStream stream = new MemoryStream(byteArray);
            diagramDC.Items.Clear();
            diagramDC.LoadDocument(stream);
        }

        private void bbi_Save_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            var stream = new MemoryStream();
            diagramDC.SaveDocument(stream);
            stream.Seek(3, SeekOrigin.Begin);
            xmlContent = stream.ReadToString();
        }

        private void bbi_tbl_NeedleGauge_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            AddTable(nameof(Table_NeedleGauge), 0, 0, new Point(0, 0));
        }

        private void bbi_tbl_AttachmentDetail_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            AddTable(nameof(Table_AttachmentDetail), 0, 0, new Point(0, 0));
        }

        private void bbi_tbl_MachineOp_ItemClick(object sender, ItemClickEventArgs e)
        {
            AddTable(nameof(Table_MachineOp), 0, 0, new Point(0, 0));
        }

        private void AddTable(string tbl, double pWidth, double pHeight, Point pPosition)
        {
            try
            {

                if (tbl == nameof(Table_MachineOp))
                {
                    if (IsDiagramItemExist_fromTag(nameof(Table_MachineOp)))
                    {
                        SelectFromTag(nameof(Table_MachineOp));
                    }
                    else
                    {
                        List<MachineFourLayout> machineList = GetMachine();
                        int rowCount = machineList.Count() + 1;

                        containerViewModel.MachineOps.Clear();

                        foreach (var item in machineList)
                        {
                            containerViewModel.MachineOps.Add(new Table_MachineOp()
                            {
                                MachineNo = item.MACHINE_NO.ToString(),
                                Seq = item.SEQ_NO.ToString(),
                                MachineType = item.MACHINE_TYPE,
                                Operation = item.OPERATION,
                                SMV = item.SMV2.ToString(),
                            });
                        }

                        DiagramContentItem machineTable = new DiagramContentItem()
                        {
                            Name = nameof(Table_MachineOp),
                            Position = pPosition,
                            Width = (pWidth > 0) ? pWidth : 5.8 * _oneFtToPxel,
                            Height = (pHeight > 0) ? pHeight : (rowCount == 0 ? 1 : (rowCount + 1)) * (_tablerowheight),
                            CustomStyleId = "containerStyle_MachineOp",
                            Content = containerViewModel,
                            Tag = nameof(Table_MachineOp),
                        };
                        diagramDC.Items.Add(machineTable);
                    }
                }
                else if (tbl == nameof(Table_NeedleGauge))
                {
                    if (IsDiagramItemExist_fromTag(nameof(Table_NeedleGauge)))
                    {
                        SelectFromTag(nameof(Table_NeedleGauge));
                    }
                    else
                    {
                        List<NeedleSummeryForLayout> needleSumGrp = GetNeedle();

                        int rowCount = needleSumGrp.Count() + 1;

                        containerViewModel.NeedleGauges.Clear();
                        foreach (var item in needleSumGrp)
                        {
                            containerViewModel.NeedleGauges.Add(new Table_NeedleGauge()
                            {
                                NeedleType = item.NEEDLE_TYPE,
                                Needle_Gauge = item.NEEDLE_GAUGE,
                                NeedleCount = item.NEEDLE_COUNT
                            });
                        }

                        DiagramContentItem needleTable = new DiagramContentItem()
                        {
                            Position = pPosition,
                            Width = (pWidth > 0) ? pWidth : 5.8 * _oneFtToPxel,
                            Height = (pHeight > 0) ? pHeight : (rowCount == 0 ? 1 : (rowCount + 1)) * (_tablerowheight),
                            CustomStyleId = "containerStyle_NeedleGauge",
                            Content = containerViewModel,
                            Tag = nameof(Table_NeedleGauge),

                        };

                        diagramDC.Items.Add(needleTable);
                    }
                }
                else if (tbl == nameof(Table_AttachmentDetail))
                {
                    if (IsDiagramItemExist_fromTag(nameof(Table_AttachmentDetail)))
                    {
                        SelectFromTag(nameof(Table_AttachmentDetail));
                    }
                    else
                    {
                        List<Attachment> tab_attachment = GetAttachment();
                        int rowCount = tab_attachment.Count() + 1;
                        containerViewModel.AttachmentDetails.Clear();
                        foreach (var item in tab_attachment)
                        {
                            containerViewModel.AttachmentDetails.Add(new Table_AttachmentDetail()
                            {
                                Operators = item.SEQ_NO,
                                Attachment = item.ATTACHMENT_CODE,
                                Quantity = item.QUANTITY
                            });
                        }

                        DiagramContentItem attachmentDetailTable = new DiagramContentItem()
                        {
                            Name = nameof(Table_AttachmentDetail),
                            Position = pPosition,
                            Width = (pWidth > 0) ? pWidth : 8 * _oneFtToPxel,
                            Height = (pHeight > 0) ? pHeight : rowCount == 0 ? 1 : rowCount * (_tablerowheight),
                            CustomStyleId = "containerStyle_AttachmentDetail",
                            Content = containerViewModel,
                            Tag = nameof(Table_AttachmentDetail),
                        };

                        diagramDC.Items.Add(attachmentDetailTable);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SelectFromTag(string ptag)
        {
            List<DiagramItem> items = diagramDC.Items.Where(x => x.Tag != null && x.Tag.ToString() == ptag).ToList();
            diagramDC.BringItemsIntoView(items, DevExpress.Diagram.Core.BringIntoViewMode.ShowAll);
            diagramDC.SelectItem(items[0]);
        }

        private bool IsDiagramItemExist_fromTag(string preffix)
        {
            return diagramDC.Items.Where(x => x.Tag != null && x.Tag.ToString().StartsWith(preffix)).Count() > 0;
        }

        private void GridControl_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = LayoutTreeHelper.GetVisualParents((DependencyObject)e.OriginalSource).OfType<GridColumnHeader>().Any();
        }

        private List<NeedleSummeryForLayout> GetNeedle()
        {
            List<NeedleSummeryForLayout> needleSumGrp = new List<NeedleSummeryForLayout>();
            for (int i = 1; i < 6; i++)
            {
                NeedleSummeryForLayout obj = new NeedleSummeryForLayout();
                obj.NEEDLE_TYPE = "TYPE - 0" + i.ToString();
                obj.NEEDLE_COUNT = i;
                obj.NEEDLE_GAUGE = i / 2;
                needleSumGrp.Add(obj);
            }
            return needleSumGrp;
        }

        private List<Attachment> GetAttachment()
        {
            List<Attachment> tab_attachment = new List<Attachment>();
            for (int i = 1; i < 6; i++)
            {
                Attachment obj = new Attachment();
                obj.ATTACHMENT_CODE = "Attachment - 0" + i.ToString();
                obj.SEQ_NO = i;
                obj.QUANTITY = i;
                tab_attachment.Add(obj);
            }
            return tab_attachment;
        }

        private List<MachineFourLayout> GetMachine()
        {
            List<MachineFourLayout> machineList = new List<MachineFourLayout>();
            for (int i = 1; i < 6; i++)
            {
                MachineFourLayout obj = new MachineFourLayout();
                obj.MACHINE_TYPE = "Type - 0" + i.ToString();
                obj.MACHINE_NO = i;
                obj.OPERATION = "Operation - 0" + i.ToString();
                obj.SEQ_NO = i;
                obj.SMV2 = i / 10;
                machineList.Add(obj);
            }
            return machineList;
        }

    }

    public class DiagramDesignerControlEx : DiagramDesignerControl
    {
        protected override IEnumerable<DevExpress.Xpf.Bars.IBarManagerControllerAction> CreateContextMenu()
        {
            DiagramShape shape = this.PrimarySelection as DiagramShape;
            // you can create your custom menu based on the currently selected item (PrimarySelection)
            List<IBarManagerControllerAction> contextMenuItems = new List<IBarManagerControllerAction>();

            if (this.PrimarySelection != null)
            {
            }
            else
            {
                BarButtonItem bbi01 = new BarButtonItem() { Content = "Select All", CommandParameter = "" };
                contextMenuItems.Add(bbi01);
                bbi01.ItemClick += SelectAllNodes;
            }

            return contextMenuItems;
        }

        private void addRotationSubMenuItems(BarSubItem barSubItem, int cellNo)
        {
            BarButtonItem menuRotationItem1 = new BarButtonItem();
            menuRotationItem1.Content = "Rotate 180 degrees";
            menuRotationItem1.ItemClick += Rotate180Angles;
            menuRotationItem1.CommandParameter = cellNo;
            barSubItem.Items.Add(menuRotationItem1);

            BarButtonItem menuRotationItem2 = new BarButtonItem();
            menuRotationItem2.Content = "Rotate 90 degrees";
            menuRotationItem2.ItemClick += Rotate90Angles;
            menuRotationItem2.CommandParameter = cellNo;
            barSubItem.Items.Add(menuRotationItem2);

            BarButtonItem menuRotationItem3 = new BarButtonItem();
            menuRotationItem3.Content = "Rotate 45 degrees";
            menuRotationItem3.ItemClick += Rotate45Angles;
            menuRotationItem3.CommandParameter = cellNo;
            barSubItem.Items.Add(menuRotationItem3);

            BarButtonItem menuRotationItem4 = new BarButtonItem();
            menuRotationItem4.Content = "Rotate 30 degrees";
            menuRotationItem4.ItemClick += Rotate30Angles;
            menuRotationItem4.CommandParameter = cellNo;
            barSubItem.Items.Add(menuRotationItem4);
        }

        void Rotate180Angles(object sender, RoutedEventArgs e)
        {
            BarButtonItem mi = (BarButtonItem)e.OriginalSource;
            int cellno = Convert.ToInt32(mi.CommandParameter.ToString());
            cell_Select(cellno);
            Commands.Execute(DiagramCommands.RotateCommand, 180);
            ClearSelection();

        }

        void Rotate90Angles(object sender, RoutedEventArgs e)
        {
            BarButtonItem mi = (BarButtonItem)e.OriginalSource;
            int cellno = Convert.ToInt32(mi.CommandParameter.ToString());
            cell_Select(cellno);
            Commands.Execute(DiagramCommands.RotateCommand, 90);
            ClearSelection();
        }

        void Rotate45Angles(object sender, RoutedEventArgs e)
        {
            BarButtonItem mi = (BarButtonItem)e.OriginalSource;
            int cellno = Convert.ToInt32(mi.CommandParameter.ToString());
            cell_Select(cellno);
            Commands.Execute(DiagramCommands.RotateCommand, 45);
            ClearSelection();
        }

        void Rotate30Angles(object sender, RoutedEventArgs e)
        {
            BarButtonItem mi = (BarButtonItem)e.OriginalSource;
            int cellno = Convert.ToInt32(mi.CommandParameter.ToString());
            cell_Select(cellno);
            Commands.Execute(DiagramCommands.RotateCommand, 30);
            ClearSelection();
        }

        private void cell_Select(int cellno)
        {

        }

        void SelectAllNodes(object sender, RoutedEventArgs e)
        {
            this.SelectItems(this.Items);
        }

        void item_2_Click(object sender, RoutedEventArgs e)
        {
            BarButtonItem mi = (BarButtonItem)e.OriginalSource;
        }
    }
}
