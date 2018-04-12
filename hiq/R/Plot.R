filepath="c:\\hea\\R\\Result1.csv"
gly = read.csv(filepath, header=TRUE, sep=",")
names(gly)
attach(gly)
 plot(HOUR, SUGAR, type='o', col="blue", main= "Time and Sugar Level", xlab="Time in Hrs", ylab="Sugar Levels", xaxt='n', yaxt='n')
axis(1, at = seq(0, 23, by = 1), las=2)
axis(2, at = seq(30, 500, by = 30), las=2)
lines(HOUR, GLYCATION, type = "o", col = "red")
legend("topright", c("Sugar Level", "Glycation Index"), lty = c(1,1),col = c("blue", "red"))