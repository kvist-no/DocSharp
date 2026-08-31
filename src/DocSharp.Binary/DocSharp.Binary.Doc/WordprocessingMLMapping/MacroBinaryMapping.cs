using System;
using DocSharp.Binary.DocFileFormat;
using DocSharp.Binary.StructuredStorage.Reader;
using DocSharp.Binary.StructuredStorage.Writer;

namespace DocSharp.Binary.WordprocessingMLMapping
{
    public class MacroBinaryMapping : DocumentMapping
    {
        public MacroBinaryMapping(ConversionContext ctx)
            : base(ctx, ctx.Docx.MainDocumentPart.VbaProjectPart)
        {
            this._ctx = ctx;
        }

        public override void Apply(WordDocument doc)
        {
            //get the Class IDs of the directories
            var macroClsid = new Guid();
            var vbaClsid = new Guid();
            foreach (var entry in doc.Storage.AllEntries)
            {
                if (string.Equals(entry.Path, "\\Macros", StringComparison.OrdinalIgnoreCase))
                {
                    macroClsid = entry.ClsId;
                }
                else if(string.Equals(entry.Path, "\\Macros\\VBA", StringComparison.OrdinalIgnoreCase))
                {
                    vbaClsid = entry.ClsId;
                }
            }

            //create a new storage
            var storage = new StructuredStorageWriter();
            storage.RootDirectoryEntry.setClsId(macroClsid);

            //copy the VBA directory
            var vba = storage.RootDirectoryEntry.AddStorageDirectoryEntry("VBA");
            vba.setClsId(vbaClsid);
            foreach (var entry in doc.Storage.AllStreamEntries)
            {
                if (entry.Path.StartsWith("\\Macros\\VBA", StringComparison.OrdinalIgnoreCase))
                {
                    vba.AddStreamDirectoryEntry(entry.Name, doc.Storage.GetStream(entry.Path));
                }
            }

            //copy the project streams
            storage.RootDirectoryEntry.AddStreamDirectoryEntry("PROJECT", doc.Storage.GetStream("\\Macros\\PROJECT"));
            storage.RootDirectoryEntry.AddStreamDirectoryEntry("PROJECTwm", doc.Storage.GetStream("\\Macros\\PROJECTwm"));

           //write the storage to the xml part
            storage.write(this._targetPart.GetStream());
        }
    }
}
