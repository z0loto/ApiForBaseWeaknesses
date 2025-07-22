using Host = ApiForBaseWeaknesses.Models.Host;

namespace ApiForBaseWeaknesses.Services;

public class ConvertToEntityService
{

    public async Task<List<Host>> ConvertToHost(Stream file)
    {
        var hosts = new List<string>();
        if (file != null)
        {
            using var reader = new StreamReader(file);
            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();
                if (!string.IsNullOrWhiteSpace(line))
                {
                    hosts.Add(line.Trim());
                }
            }
        }

        var result = Mapping.ImportMapper.MapToHost(hosts);
        return result;
    }
}