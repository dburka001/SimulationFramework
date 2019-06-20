InterpolateMortality <- function(inputArray) {
    dat <- inputArray
    #dat[length(dat)] = 1
    output <- dat

    non.na.pos <- which(!is.na(dat))
    na.pos <- which(is.na(dat))

    for (i in na.pos) {
        minId = non.na.pos[max(which((non.na.pos < i) == TRUE))]
        maxId = non.na.pos[min(which((non.na.pos < i) == FALSE))]
        multiplier = (i - minId) / (maxId - minId)
        output[i] = dat[minId] + (dat[maxId] - dat[minId]) * multiplier
    }

    return(output)
}

InterpolateMortality(inputArray)