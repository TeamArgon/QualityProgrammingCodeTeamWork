namespace Minesweeper
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class Field
    {
        private int value;
        private FieldStatus status;

        public int Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        public FieldStatus Status
        {
            get { return this.status; }
            set { this.status = value; }
        }

        public enum FieldStatus 
        { 
            Closed,
            Opened,
            IsAMine
        }

        public Field()
        {
            this.value = 0;
            this.status = FieldStatus.Closed;
        }
    }
}
