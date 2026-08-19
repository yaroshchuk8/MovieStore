namespace MovieStore.Api.Common.DTOs;

public record FileDescriptor(Stream Content, string Extension, long SizeBytes);