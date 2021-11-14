# MakelaarCounter API
This API was built to consume Funda's "Aanbod" (offers, in english) service. In it, there are three open endpoints:
- /makelaars/offer-count/{searchQuery}/{top}
  - Retrieves the {top} makerlaars with most house for sale offers based on the search query.
- /makelaars/offer-count-amsterdam
  - Retrieves the top 10 makelaars with most house for sale offers in Amsterdam.
- /makelaars/offer-count-amsterdam
  - Retrieves the top 10 makelaars with most house for sale offers in Amsterdam that contain a garder (in dutch, tuin).

## Notes
- There is a swagger UI included. If you run the project using the port 5000 it is [here](http://localhost:5000/swagger/index.html)
- The api key (FundaApi:ApiKey in the cofiguration) was not commited to not expose it to public
 
