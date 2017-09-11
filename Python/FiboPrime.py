a, b = 0, 1
print("Fibo Series:")
while b < 100:
	print b
	a, b = b, a+b
	

print("\nPrime Nos:")
lower, upper  = 1, 100
for num in range(lower,upper + 1):
   if num > 1:
       for i in range(2,num):
           if (num % i) == 0:
               break
       else:
           print(num)