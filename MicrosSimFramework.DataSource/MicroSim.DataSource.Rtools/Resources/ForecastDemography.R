if (!"demography" %in% installed.packages()) { install.packages("demography") }
library("demography")

ForecastDemography <- function(type, mortality, pop, ages, years, forecastYears) {
    data <- demogdata(mortality, pop, ages, years, type, "Forecast", "Total")
    lcaModel <- lca(data, series = "Total", max.age = max(ages), interpolate=TRUE)
    lcaResult <- forecast(lcaModel, h = forecastYears)
    output <- lcaResult$rate$Total
    return(list("output" = output, "kt_lower" = lcaResult$kt.f$lower, "kt" = lcaResult$kt.f$x, "kt_upper" = lcaResult$kt.f$upper))
}

ForecastDemography(type, mortality, pop, ages, years, forecastYears)