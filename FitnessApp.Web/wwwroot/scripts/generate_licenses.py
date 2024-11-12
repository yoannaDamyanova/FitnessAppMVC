import random
import json

# Check if a number has repeating digits
def has_repeating_digits(num):
    return len(set(str(num))) != len(str(num))

def generate_license_numbers(count=50):
    license_numbers = {123456}  # Start with 123456 as the first license number
    while len(license_numbers) < count:
        license_number = random.randint(100000, 999999)  # Generate a 6-digit number
        if not has_repeating_digits(license_number):  # Check if digits are unique
            license_numbers.add(license_number)
    return list(license_numbers)

# Generate 50 unique license numbers
license_numbers = generate_license_numbers(50)

# Save the license numbers to a JSON file
with open('FitnessApp.Web/wwwroot/data/license_numbers.json', 'w') as json_file:
    json.dump(license_numbers, json_file)

print("License numbers saved to 'license_numbers.json'")