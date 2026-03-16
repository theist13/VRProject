# Farming Grid Migration Guide

## ของเดิมที่เก็บไว้ใช้ต่อได้
- ใช้แนวคิด `OnTriggerEnter` จาก `CookingPlate` ต่อได้ตรง ๆ สำหรับตรวจจับวัตถุที่ปล่อยลงแปลง
- ใช้ `XRGrabInteractable` แบบเดิมเพื่อเช็คว่า "ยังถืออยู่" (`isSelected`) ก่อนยอมรับการวาง
- ใช้ ScriptableObject pattern เหมือน `RecipeSO` แต่เปลี่ยนเป็น `CropDefinitionSO` เพื่อเก็บข้อมูลพืช

## ของที่ควรแยก/ลดบทบาท
- `CookingPlate`, `Ingredient`, `RecipeSO`, `IngredientType` เป็นระบบทำอาหาร ควรแยก scene หรือโฟลเดอร์ถ้าเกมหลักเปลี่ยนเป็นปลูกพืช
- ถ้าไม่ใช้งานระบบทำอาหารแล้ว ให้ถอด reference ใน Scene ก่อนลบไฟล์ เพื่อไม่ให้ Missing Script

## Mapping โครงใหม่
- เมล็ด: `SeedItem`
- ช่องกริด: `GridPlot`
- ข้อมูลพืช: `CropDefinitionSO`
- วัตถุผลผลิต: `HarvestItem` (+ `PlotOwnership`)
- ตะกร้าขาย: `HarvestBasket`
- เงินผู้เล่น: `PlayerWallet`

## ลำดับการทำงาน
1. ผู้เล่นปล่อยเมล็ดลง `GridPlot`
2. เมล็ด snap เข้าจุด และแปลงถูกล็อกไม่ให้วางซ้อน
3. รอเวลา -> ต้นอ่อน -> ผลผลิต
4. หยิบผลผลิตไปใส่ `HarvestBasket`
5. ระบบเพิ่มเงินเข้า `PlayerWallet`
