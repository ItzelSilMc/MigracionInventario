// Decompiled with JetBrains decompiler
// Type: ComboboxItem
// Assembly: VKMIGINV, Version=1.0.0.4, Culture=neutral, PublicKeyToken=null
// MVID: 091BB591-6A6D-4391-A9A1-65EF14350A86
// Assembly location: C:\MigracionInventario\VKMIGINV.exe

public class ComboboxItem
{
    public string Text { get; set; }

    public object Value { get; set; }

    public override string ToString()
    {
        return this.Text;
    }
}
