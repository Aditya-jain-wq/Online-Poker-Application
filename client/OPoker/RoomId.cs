internal struct RoomId {
    public int id;

    public RoomId(string room_id) {
        if (room_id.Length != 6) {
            throw new System.ArgumentException("Incorrect Length");
        }
        try {
            id = int.Parse(room_id, System.Globalization.NumberStyles.AllowHexSpecifier);
        } catch (System.Exception) {
            throw new System.ArgumentException("Incorrect Format");
        }
    }
}
