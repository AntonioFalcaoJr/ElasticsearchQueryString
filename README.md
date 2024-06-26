# Elasticsearch Query String Web API

This project is a web API that demonstrates the use of Elasticsearch query string capabilities. The API allows users to input query strings, select fields and indexes, and retrieve search results with highlighted terms.

### Features

- **Search Request Handling**: Accepts Elasticsearch query strings.
- **Field and Index Selection**: Choose fields and indexes to search within.
- **Highlighted Results**: Displays search results with highlighted terms.

### Usage

1. **Seed Data**: Use the `/seed` endpoint to populate Elasticsearch with initial data.
2. **Search**: Use the `/search` endpoint with query parameters to perform searches. The results will include highlighted terms matching the query.

> Check the syntax of query strings [here](Syntax.md).   
> For more information on Elasticsearch query strings, refer to the [official documentation](https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-query-string-query.html).

## Running

Ensure Docker is installed and running. Use the provided [docker-compose.yml](docker-compose.yml) file to start the application and Elasticsearch stack.

```sh
docker-compose --profile all up -d
```

The `ports` section in each service maps the internal container ports to the host machine ports. For example:
- `Webapi` is accessible on [http://localhost:5000](http://localhost:5000)
- `Elasticsearch` is accessible on [http://localhost:9200](http://localhost:9200)
- `Kibana` is accessible on [http://localhost:5601](http://localhost:5601)

### Running Specific Profiles

To run only the application (`webapi`), use:

```sh
docker-compose --profile app up
```

To run only the infrastructure (`elasticsearch` and `kibana`), use:

```sh
docker-compose --profile infra up
```
## API Endpoints

### `POST /seed`

Seeds initial data into Elasticsearch, including documents for `Person`, `Company`, and `Product`.

```csharp
public record Person(Guid Id, Guid TenantId, string Name, string Email);
public record Company(Guid Id, Guid TenantId, string Name, string Address);
public record Product(Guid Id, Guid TenantId, string Name, decimal Price);
```

### `GET /search`

Searches the specified fields and indexes based on the provided query string.

#### Parameters

- **Query** (string): The query string to search for.
- **Fields** (array of strings): The fields to search within.
- **Indexes** (array of strings): The indexes to search against.

#### Request

![](.assets/search.png)

#### Response

```json
{
  "items": [
    {
      "document": {
        "id": "b37feba7-799b-6d24-1d2a-893a45189299",
        "tenantId": "315ea505-bf6f-4f04-93de-f15becc035ab",
        "name": "Batz Group",
        "address": "0306 Hardy Viaduct"
      },
      "highlights": {
        "address": [
          "0306 <em>Hardy</em> Viaduct"
        ]
      }
    },
    {
      "document": {
        "id": "f8ed9fe5-ed9d-6586-f4c7-cbfd3a43c5e1",
        "tenantId": "315ea505-bf6f-4f04-93de-f15becc035ab",
        "name": "Sleek Cotton Chips",
        "price": "484.44"
      },
      "highlights": {
        "name": [
          "<em>Sleek</em> Cotton Chips"
        ]
      }
    }
  ],
  "page": {
    "number": 1,
    "size": 2,
    "hasPrevious": false,
    "hasNext": true
  }
}
```
