using Host = ApiForBaseWeaknesses.Models.Host;

namespace ApiForBaseWeaknesses.Services;

public class ConvertToEntityService
{

    public async Task<List<Host>> ConvertToEntity(IFormFile? file)
    {
        List<string> hosts = new();
        if (file != null)
        {
            using var reader = new StreamReader(file.OpenReadStream());
            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();
                if (!string.IsNullOrWhiteSpace(line))
                {
                    hosts.Add(line.Trim());
                }
            }
        }

        List<Host> result = Mapping.ImportMapper.MapToHost(hosts);
        return result;
    }
}