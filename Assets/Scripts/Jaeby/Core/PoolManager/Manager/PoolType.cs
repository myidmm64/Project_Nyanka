public enum PoolType/*풀링 타입(풀링할 에들이 많아지면 계속 추가*/
{
    Sound,
    Bullet,
    BulletImpact,
    BulletExplosionImpact,
    BulletMuzzleImpact,
    PopupText,
    EXPBall,
    EnemyDeadEffect,

    #region Enemy
    ComonEnemy,
    DashEnemy,
    CannonEnemy,
    ExplosionEnemy,
    #endregion

    EnemyExplosionEffect,

    #region Mortar Boss
    BulletBossCommonBullet,
    BulletBossFirecrackerBullet,
    BulletBossMortarBullet,
    BulletBossMortarEffect,
    #endregion
}