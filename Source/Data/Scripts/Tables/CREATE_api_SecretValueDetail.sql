CREATE TABLE api.SecretValueDetail(
	[Source] varchar(10) NOT NULL,
	[Type] varchar(15) NOT NULL,
	CONSTRAINT
		FK_SecretValueDetail_SecretSource_Source_Source
	FOREIGN KEY
		([Source])
	REFERENCES
		api.SecretSource([Source]),
	CONSTRAINT
		UNQ_SecretValueDetail_Source_Type
	UNIQUE
		(Source, Type)
);