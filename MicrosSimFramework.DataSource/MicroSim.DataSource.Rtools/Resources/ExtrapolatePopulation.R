ExtrapolatePopulation <- function(startValue, steps, totalValue) {
    Gompertz <- function(x, a, c) {
        (exp(-c * (exp(a * x) - 1)))
    }

    TargetFunction <- function(p) {
        g <- startValue * Gompertz(x, p[1], p[2])
        abs(sum(round(g, digits = 0)) - totalValue)
    }

    x <- 0:steps
    a = 0.5
    c = 0.5

    par = c(a, c)

    result <- optim(par, TargetFunction)$par
    
    return(startValue * Gompertz(x, result[1], result[2]))
}

ExtrapolatePopulation(startValue, steps, totalValue)