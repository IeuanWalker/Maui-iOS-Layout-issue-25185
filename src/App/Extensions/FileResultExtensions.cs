namespace App.Extensions;

public static class FileResultExtensions
{
	public static bool IsImage(this FileResult file)
	{
		// Check the image mime types
		if(file.ContentType == null)
		{
			return false;
		}

		if(
			file.ContentType != "image/jpg" && file.ContentType != "jpg" &&
			file.ContentType != "image/jpeg" && file.ContentType != "jpeg" &&
			file.ContentType != "image/pjpeg" && file.ContentType != "pjpeg" &&
			file.ContentType != "image/x-png" && file.ContentType != "x-png" &&
			file.ContentType != "image/png" && file.ContentType != "png" &&
			file.ContentType != "image/bmp" && file.ContentType != "bmp" &&
			file.ContentType != "image/x-ms-bmp" && file.ContentType != "x-ms-bmp" &&
			file.ContentType != "image/x-bmp" && file.ContentType != "x-bmp" &&
			file.ContentType != "image/heic" && file.ContentType != "heic")
		{
			return false;
		}

		// Check the image extension
		string extension = Path.GetExtension(file.FileName);
		if(extension == null)
		{
			return false;
		}

		extension = extension.ToLower();
		return extension == ".png"
			   || extension == ".jpg"
			   || extension == ".jpeg"
			   || extension == ".jpe"
			   || extension == ".bmp"
			   || extension == ".dib"
			   || extension == ".heic";
	}
}