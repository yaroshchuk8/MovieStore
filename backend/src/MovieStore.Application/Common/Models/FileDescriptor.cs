namespace MovieStore.Application.Common.Models;

public record FileDescriptor(Stream Content, string Extension, string ContentType, long SizeBytes);