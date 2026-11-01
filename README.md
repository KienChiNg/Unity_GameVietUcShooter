# VR Shooter — Đấu súng VR trình diễn độ trễ mạng 5G

Game bắn súng góc nhìn thứ nhất (FPS) trên kính **Meta Quest**, chơi online nhiều người theo thời gian thực qua **Photon PUN 2**. Game được làm cho [AVSTC Lab](https://avstc.ptit.edu.vn/labs/) (Trung tâm Công nghệ Chiến lược Úc – Việt) để người xem cảm nhận trực tiếp độ trễ mạng: hai người chơi đọ tốc độ bóp cò, ai có đường truyền nhanh hơn sẽ thắng.

## Demo

**Bản phát triển**

https://github.com/user-attachments/assets/aa8cd414-24fc-44b9-8f03-bb2cf5761526

**Bản hoàn thiện**

https://github.com/user-attachments/assets/526bb909-9fe2-488c-8d8e-35afc90d3d33

## Luật chơi

| Vai trò | Thiết bị | Nhiệm vụ |
| --- | --- | --- |
| Quản trị (Master Client) | PC | Tạo phòng, bắt đầu / chơi lại, nhận phát bắn và công bố người thắng |
| Player 1 | Meta Quest | Đấu súng qua đường truyền 5G (ping thực tế) |
| Player 2 | Meta Quest | Đấu súng qua đường truyền chậm hơn (giả lập thêm độ trễ) |

1. Quản trị tạo phòng (tối đa 3 người). Hai người chơi chọn phòng trong danh sách để vào.
2. Khi quản trị bấm bắt đầu, cả hai bóp cò tay cầm để bắn.
3. Phát bắn đến máy quản trị trước được tính là thắng. Màn hình kết quả hiện người thắng, thời gian phản ứng và ping của từng bên.
4. Quản trị có thể cho chơi lại (rematch) ngay trong phòng.

## Kỹ thuật chính

- **Tích hợp VR:** plugin Oculus (Meta XR SDK 71, Unity XR Management, Oculus XR Plugin). Đọc trigger và nút của tay cầm qua `UnityEngine.XR.InputDevice`.
- **Multiplayer Photon PUN 2:**
  - Kết nối Photon Cloud (region Asia), hoặc tự host Photon Server trong mạng LAN bằng cách nhập địa chỉ IP (chế độ offline).
  - Danh sách phòng cập nhật realtime; Custom Properties lưu vai trò người chơi và chủ phòng.
  - Các sự kiện bắt đầu, bắn, thắng, chơi lại gửi bằng `PhotonNetwork.RaiseEvent` với event code riêng, chế độ reliable.
  - Ping đồng bộ liên tục qua `IPunObservable` để hiển thị cho mọi người.
  - Tự xử lý khi người chơi hoặc chủ phòng rời phòng, hoặc khi Master Client bị chuyển sang máy khác.
- **Giả lập độ trễ:** Player 2 được cộng thêm 50–70 ms vào ping và trễ phát bắn 0,4–1 giây, tạo khác biệt rõ giữa hai đường truyền.
- **Phân xử công bằng:** chỉ Master Client quyết định người thắng, theo phát bắn đến trước.

## Yêu cầu

- Unity **2022.3** (URP)
- Meta Quest 2 / 3 / Pro
- Photon PUN 2: một AppId Photon Realtime, hoặc Photon Server tự host
- Gói **Meta Movement 71.0.1**: `Packages/manifest.json` đang trỏ tới một đường dẫn local. Hãy tải gói về và sửa lại đường dẫn trước khi mở dự án.

## Chạy dự án

1. Sửa đường dẫn `com.meta.movement` trong `Packages/manifest.json`, rồi mở dự án bằng Unity 2022.3.
2. Điền AppId vào *Window → Photon Unity Networking → PUN Wizard* (hoặc `PhotonServerSettings`).
3. Build Android cho Meta Quest với scene `Assets/Scenes/Player/ScenePlayer.unity`. Build PC cho máy quản trị với scene `Assets/Scenes/Player/SceneAdministrator.unity`.
4. Mở bản quản trị để tạo phòng, rồi cho hai kính Quest vào phòng.

## Cấu trúc thư mục

```
Assets/
  Scenes/Player/        ScenePlayer (Quest), SceneAdministrator (PC)
  Scripts/New Script/   Launcher, quản lý phòng, đồng bộ phát bắn, game state, XR input
  Scripts/Common/       Cấu hình kết nối, entity dùng chung
  Oculus/, XR/          Cấu hình Meta XR
  Prefabs/, Resources/  Nhân vật, súng, UI
```
