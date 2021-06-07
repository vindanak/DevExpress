using System;
using System.Collections.ObjectModel;

namespace Layout
{
    [Serializable]
    public class ContainerViewModel
    {
        public ObservableCollection<Table_MachineOp> MachineOps
        {
            get;
            set;
        }
        public ObservableCollection<Table_NeedleGauge> NeedleGauges
        {
            get;
            set;
        }
        public ObservableCollection<Table_AttachmentDetail> AttachmentDetails
        {
            get;
            set;
        }

        public ContainerViewModel()
        {
            MachineOps = new ObservableCollection<Table_MachineOp>();
            NeedleGauges = new ObservableCollection<Table_NeedleGauge>();
            AttachmentDetails = new ObservableCollection<Table_AttachmentDetail>();

        }

        [Serializable]
        public class Table_MachineOp
        {
            public string MachineNo
            {
                get;
                set;
            }
            public string Cell
            {
                get;
                set;
            }

            public string Seq
            {
                get;
                set;
            }

            public string MachineType
            {
                get;
                set;
            }

            public string Operation
            {
                get;
                set;
            }

            public string SMV
            {
                get;
                set;
            }
        }

        [Serializable]
        public class Table_NeedleGauge
        {
            public string NeedleType
            {
                get;
                set;
            }

            public double Needle_Gauge
            {
                get;
                set;
            }

            public int NeedleCount
            {
                get;
                set;
            }
        }

        [Serializable]
        public class Table_AttachmentDetail
        {
            public int Operators
            {
                get;
                set;
            }

            public string Attachment
            {
                get;
                set;
            }

            public int Quantity
            {
                get;
                set;
            }
        }

    }
}
