namespace notifier.Domain.Dto;


public class PingDto
{
    public string? Address {  get; set; }
    
    public string? RoundTriptime {  get; set; }

    public string? Ttl {  get; set; }

    public string? DontFragment {  get; set; }

    public string? BufferSize {  get; set; }

    public int SuccessPercent { get; set; }
}