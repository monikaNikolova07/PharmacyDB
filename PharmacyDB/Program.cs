namespace PharmacyDB;
    using PharmacyDB.Models;
{
    internal class Program
    {
        static void Main(string[] args)
        {
        using (var context = new PharmacyDBContext())
        {
            Console.WriteLine("=== Всички лекарства ===");
            var medicines = context.Medicines.ToList();
            foreach (var med in medicines)
                Console.WriteLine($"{med.Name} - {med.Manufacturer} - {med.Price:C} - {med.QuantityInStock} бр.");

            Console.WriteLine("\n=== Лекарства с наличност под 50 ===");
            var lowStock = context.Medicines.Where(m => m.QuantityInStock < 50).ToList();
            foreach (var med in lowStock)
                Console.WriteLine($"{med.Name} - {med.QuantityInStock} бр.");

            Console.WriteLine("\n=== Най-скъпите 3 лекарства ===");
            var top3Expensive = context.Medicines.OrderByDescending(m => m.Price).Take(3).ToList();
            foreach (var med in top3Expensive)
                Console.WriteLine($"{med.Name} - {med.Price:C}");

            Console.WriteLine("\n=== Всички служители ===");
            var employees = context.Employees.ToList();
            foreach (var emp in employees)
                Console.WriteLine($"{emp.Name} - {emp.Position} - {emp.Salary:C}");

            Console.Write("\nВъведете име на доставчик: ");
            string supplierName = Console.ReadLine();
            var supplierOrders = context.Orders.Where(o => o.SupplierName == supplierName).ToList();
            Console.WriteLine($"=== Поръчки от {supplierName} ===");
            foreach (var order in supplierOrders)
                Console.WriteLine($"Лекарство ID: {order.MedicineId} - {order.OrderDate} - {order.QuantityOrdered} бр.");

            Console.WriteLine("\n=== Лекарства с изчерпано количество ===");
            var outOfStock = context.Medicines.Where(m => m.QuantityInStock == 0).ToList();
            foreach (var med in outOfStock)
                Console.WriteLine($"{med.Name} е изчерпано!");

            Console.WriteLine("\n=== Имена на лекари, които са изписвали рецепти ===");
            var doctorNames = context.Prescriptions.Select(p => p.DoctorName).Distinct().ToList();
            foreach (var doc in doctorNames)
                Console.WriteLine(doc);

            Console.Write("\nВъведете име на лекар: ");
            string doctorInput = Console.ReadLine();
            var patients = context.Prescriptions.Where(p => p.DoctorName == doctorInput).Select(p => p.PatientName).Distinct().ToList();
            Console.WriteLine($"=== Пациенти на {doctorInput} ===");
            foreach (var patient in patients)
                Console.WriteLine(patient);

            Console.WriteLine("\n=== Обща стойност на всички поръчки ===");
            decimal totalOrderValue = context.Orders
                .Include(o => o.Medicine)
                .Sum(o => o.Medicine.Price * o.QuantityOrdered);
            Console.WriteLine($"Обща стойност: {totalOrderValue:C}");
        }
    }
}
