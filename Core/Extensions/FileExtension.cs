namespace Core.Extensions;

/// <summary>
/// 文件扩展
/// </summary>
public static class FileExtension
{
    /// <summary>
    /// 判断文件是否存在
    /// </summary>
    /// <param name="path">文件路径</param>
    /// <returns></returns>
    public static bool FileIsExists(this string path) => File.Exists(path);

    /// <summary>
    /// 删除文件
    /// </summary>
    /// <param name="path">文件路径</param>
    public static void DeleteFile(this string path)
    {
        if (path.FileIsExists())
            File.Delete(path);
    }
}
